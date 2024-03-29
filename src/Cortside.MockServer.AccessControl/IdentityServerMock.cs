using System.IO;
using Cortside.MockServer.AccessControl.Models;
using Cortside.MockServer.Builder;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Cortside.MockServer.AccessControl {
    public class IdentityServerMock : IMockHttpMock {
        private readonly IdsDiscovery idsConfiguration;
        private readonly IdsJwks idsJwks;

        public IdentityServerMock(string discoveryFilename, string jwksFilename) {
            idsConfiguration = JsonConvert.DeserializeObject<IdsDiscovery>(File.ReadAllText(discoveryFilename));
            idsJwks = JsonConvert.DeserializeObject<IdsJwks>(File.ReadAllText(jwksFilename));
        }

        public void Configure(MockHttpServer server) {
            idsConfiguration.Authorization_endpoint = idsConfiguration.Authorization_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Issuer = idsConfiguration.Issuer.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Jwks_uri = idsConfiguration.Jwks_uri.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Token_endpoint = idsConfiguration.Token_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Userinfo_endpoint = idsConfiguration.Userinfo_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.End_session_endpoint = idsConfiguration.End_session_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Check_session_iframe = idsConfiguration.Check_session_iframe.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Revocation_endpoint = idsConfiguration.Revocation_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Introspection_endpoint = idsConfiguration.Introspection_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);
            idsConfiguration.Device_authorization_endpoint = idsConfiguration.Device_authorization_endpoint.Replace("https://identityserver.cortside.com", server.Urls[0]);

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/.well-known/openid-configuration")
                    .UsingGet()
                    )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_ => JsonConvert.SerializeObject(idsConfiguration))
                    );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/.well-known/openid-configuration/jwks")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_ => JsonConvert.SerializeObject(idsJwks))
                );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/connect/token")
                        .UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_ => JsonConvert.SerializeObject(new AuthenticationResponseModel {
                            TokenType = "Bearer",
                            ExpiresIn = "3600",
                            AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjY1QjBCQTk2MUI0NDMwQUJDNTNCRUI5NkVDMjBDNzQ5OTdGMDMwMzIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJaYkM2bGh0RU1LdkZPLXVXN0NESFNaZndNREkifQ.eyJuYmYiOjE1ODI5MzUwMDcsImV4cCI6MTU4MjkzODYwNywiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eXNlcnZlci5rOHMuZW5lcmJhbmsuY29tIiwiYXVkIjpbImh0dHBzOi8vaWRlbnRpdHlzZXJ2ZXIuazhzLmVuZXJiYW5rLmNvbS9yZXNvdXJjZXMiLCJjb21tb24uY29tbXVuaWNhdGlvbnMiLCJkb2N1bWVudC1hcGkiLCJlYm9hLndlYmFwaSIsImZ1bmRpbmdtYW5hZ2VtZW50LmFwaSIsInBvbGljeXNlcnZlci5ydW50aW1lIiwidXNlci1hcGkiXSwiY2xpZW50X2lkIjoibG9hbi1hcGkiLCJyb2xlIjoibG9hbi1hcGkiLCJzdWIiOiI4MjgwZmIxNi0wNzdiLTQ5M2EtYmRhZi01MGY2MmU2ZDZhZTkiLCJncm91cHMiOiJlY2JkYTNmNy1kNjI4LTRlNWEtODc5Yy1jZjZjYWFkMjNiYTEiLCJzY29wZSI6WyJjb21tb24uY29tbXVuaWNhdGlvbnMiLCJkb2N1bWVudC1hcGkiLCJlYm9hLndlYmFwaSIsImZ1bmRpbmdtYW5hZ2VtZW50LmFwaSIsInBvbGljeXNlcnZlci5ydW50aW1lIiwidXNlci1hcGkiXX0.O5M1tMO1tUqex8UngnjwEwE8BvQg_mq8gmwEiR0kEXKxZcbLc0lL6cNTxIZxNLkhI7Xi5t-6kpNOuKGhYg3X8NvRci9S5kZJNR-zYpphIIiykHn0lAKHLZV4DbP4EJMW8K21U3Drz7i2YnQ0xV7KJFJfwuZCiaxauaq2Px1J5_Ef4LB8etwWfv0FLH3xw-Mp9E91fL9FeLxhmsBsMyNPIXWZlby-KIv8zxnPV-vRqFh0scuzn3NjS6VELO6GDsCfTzuuytfNHc1M7OOrmn-xrQTIGH-_FHYFU-ezwSsOH9aTphcWFXDw9NQveDBCX-uJaDphktkvcuYPiJ9XHZQhYQ"
                        }))
                );
        }
    }
}
