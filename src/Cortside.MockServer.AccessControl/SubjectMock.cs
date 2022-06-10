using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cortside.MockServer.AccessControl.Models;
using Newtonsoft.Json;
using Serilog;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Cortside.MockServer.AccessControl {
    public class SubjectMock : IMockHttpServerBuilder {
        private readonly Subjects subjects;

        public SubjectMock(string filename) {
            subjects = JsonConvert.DeserializeObject<Subjects>(File.ReadAllText(filename));
        }

        public SubjectMock(Subjects subjects) {
            this.subjects = subjects;
        }

        public void Configure(WireMockServer server) {
            foreach (var subject in subjects.SubjectsList) {
                Log.Logger.Debug("Setting up client: {ClientId}", subject.ClientId);
                var claims = new List<SubjectClaim>();
                claims.AddRange(subject.Claims);
                claims.Add(new SubjectClaim { Type = "sub", Value = subject.SubjectId });
                claims.Add(new SubjectClaim { Type = "upn", Value = subject.ClientId });
                var dictClaims = claims.ToDictionary(x => x.Type, x => x.Value);
                var claimsJson = JsonConvert.SerializeObject(dictClaims);

                foreach (var policy in subject.Policies) {
                    server
                        .Given(
                        Request.Create().WithPath($"/runtime/policy/{policy.PolicyName}")
                        .WithBody(b => b?.Contains(subject.SubjectId) == true)
                            .UsingPost()
                        )
                        .RespondWith(
                            Response.Create()
                                .WithStatusCode(200)
                                .WithBody(_ => JsonConvert.SerializeObject(policy.Authorization))
                        );
                }

                server
                    .Given(
                        Request.Create().WithPath("/connect/token")
                            .WithBody(b => b?.Contains(subject.ClientId) == true)
                            .UsingPost()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(_ => JsonConvert.SerializeObject(new AuthenticationResponseModel {
                                TokenType = "Bearer",
                                ExpiresIn = "3600",
                                AccessToken = subject.ReferenceToken
                            }))
                );
                server
                    .Given(
                        Request.Create().WithPath("/connect/token")
                            .WithHeader(h => h.ContainsKey("Authorization") && h["Authorization"]?.FirstOrDefault() != null && h["Authorization"]?.FirstOrDefault().Replace("Basic ", "").DecodeBase64().Contains(subject.ClientId) == true)
                            .UsingPost()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "application/json")
                            .WithBody(_ => JsonConvert.SerializeObject(new AuthenticationResponseModel {
                                TokenType = "Bearer",
                                ExpiresIn = "3600",
                                AccessToken = subject.ReferenceToken
                            }))
                );

                server
                    .Given(
                        Request.Create().WithPath("/connect/introspect")
                            .WithBody(b => b?.Contains(subject.ReferenceToken) == true)
                            .UsingPost()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithBody(_ => claimsJson)
                    );
            }
        }
    }
}
