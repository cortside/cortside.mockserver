using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Cortside.MockServer.Mocks {
    public class HttpStatusesMock : IMockHttpMock {
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
        }
    }
}
