using Serilog;
using WireMock.Server;
using WireMock.Settings;
using WireMock.Types;

namespace Cortside.MockServer {
    public class MockHttpServer {
        private readonly WireMockServer mockServer;

        public MockHttpServer(int? port = null, ILogger logger = null) {
            var settings = new WireMockServerSettings {
                CorsPolicyOptions = CorsPolicyOptions.AllowAll,
                StartAdminInterface = true,
                AllowCSharpCodeMatcher = true
            };

            if (port != null) {
                settings.Port = port;
            }

            if (logger != null) {
                settings.Logger = new WireMockLogger(logger);
            }

            mockServer = WireMockServer.Start(settings);
        }

        public MockHttpServer(string routePrefix, int? port = null, ILogger logger = null) : this(port, logger) {
            RoutePrefix = routePrefix;
        }

        public string[] Urls => mockServer.Urls;

        public string Url => mockServer.Urls[0];

        public string RoutePrefix { get; }

        public string RoutePrefixUrl => $"{Url}/{RoutePrefix}";

        public bool IsStarted { get; internal set; }

        public MockHttpServer ConfigureBuilder<T>() where T : IMockHttpServerBuilder, new() {
            new T().Configure(mockServer);
            return this;
        }

        public MockHttpServer ConfigureBuilder<T>(T instance) where T : IMockHttpServerBuilder {
            instance.Configure(mockServer);
            return this;
        }

        public void WaitForStart() {
            while (!mockServer.IsStarted) {
                // nothing to do here
            }
            IsStarted = true;
        }

        public void Stop() {
            mockServer.Stop();
        }
    }
}
