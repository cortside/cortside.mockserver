using Cortside.MockServer.Builder;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Cortside.MockServer.Mocks {
    public class HttpStatusesMock : IMockHttpMock {
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
        }
    }
}
