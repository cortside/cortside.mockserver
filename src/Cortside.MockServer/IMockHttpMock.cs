using WireMock.Server;

namespace Cortside.MockServer {
    public interface IMockHttpMock {
        public void Configure(WireMockServer server);
    }
}
