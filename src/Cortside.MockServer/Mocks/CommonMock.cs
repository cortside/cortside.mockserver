using System;
using Cortside.MockServer.Builder;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Util;

namespace Cortside.MockServer.Mocks {
    public class CommonMock : IMockHttpMock {
        public void Configure(MockHttpServer server) {
            server.WireMockServer.AddCatchAllMapping();
            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/".Split('?')[0]).UsingGet()
                    )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                    );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/api/health").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithTransformer()
                        .WithBody("{\"service\":\"policyserver\",\"status\":\"OK\",\"healthy\":true,\"timestamp\":\"" + DateTime.UtcNow.ToString("o") +
                                  "\",\"build\":{\"timestamp\":\"2023-04-07T17:32:18.3195Z\",\"version\":\"1.64.1684\",\"tag\":\"1.64.1684\",\"suffix\":\"\"}}")
                        .WithStatusCode(200)
                );

            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/health").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithTransformer()
                        .WithBody("{\"service\":\"policyserver\",\"status\":\"OK\",\"healthy\":true,\"timestamp\":\"" + DateTime.UtcNow.ToString("o") +
                                  "\",\"build\":{\"timestamp\":\"2023-04-07T17:32:18.3195Z\",\"version\":\"1.64.1684\",\"tag\":\"1.64.1684\",\"suffix\":\"\"}}")
                        .WithStatusCode(200)
                );

            server.WireMockServer.Given(Request.Create().WithPath("/api/sap")
                .UsingPost()
                .WithBody((IBodyData xmlData) => {
                    //xmlData is always null
                    return true;
                }))
                .RespondWith(Response.Create().WithStatusCode(System.Net.HttpStatusCode.OK));

            server.WireMockServer
                .Given(Request.Create()
                    .UsingAnyMethod())
                .RespondWith(Response.Create()
                    .WithTransformer()
                    .WithBody("{{Random Type=\"Integer\" Min=100 Max=999999}} {{DateTime.UtcNow}} {{DateTime.UtcNow \"yyyy-MMM\"}} {{String.Format (DateTime.UtcNow) \"MMM-dd\"}}"));
        }
    }
}
