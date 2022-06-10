using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Util;

namespace Cortside.MockServer {
    public class CommonMock : IMockHttpServerBuilder {
        public void Configure(WireMockServer server) {
            server.AddCatchAllMapping();
            server
                .Given(
                    Request.Create().WithPath("/".Split('?')[0]).UsingGet()
                    )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                    );

            server
                .Given(
                    Request.Create().WithPath("/").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            server
                .Given(
                    Request.Create().WithPath("/api/health").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                );

            server.Given(Request.Create().WithPath("/api/sap")
                .UsingPost()
                .WithBody((IBodyData xmlData) => {
                    //xmlData is always null
                    return true;
                }))
                .RespondWith(Response.Create().WithStatusCode(System.Net.HttpStatusCode.OK));

            server
                .Given(Request.Create()
                    .UsingAnyMethod())
                .RespondWith(Response.Create()
                    .WithTransformer()
                    .WithBody("{{Random Type=\"Integer\" Min=100 Max=999999}} {{DateTime.Now}} {{DateTime.Now \"yyyy-MMM\"}} {{String.Format (DateTime.Now) \"MMM-dd\"}}"));
        }
    }
}
