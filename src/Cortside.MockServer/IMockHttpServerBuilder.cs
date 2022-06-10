using WireMock.Server;

namespace Cortside.MockServer {
    public interface IMockHttpServerBuilder {
        public void Configure(WireMockServer server);
    }
}
