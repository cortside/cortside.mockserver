using System;
using Microsoft.Extensions.Logging;
using WireMock.Logging;

namespace Cortside.MockServer {
    public class MockHttpServerBuilder : IMockHttpServerBuilder {
        private bool hostBuilt;

        public MockHttpServerBuilder(int? port, ILogger logger) {
            Options = new MockHttpServerOptions() {
                Port = port
            };
            if (logger != null) {
                Options.Logger = new WireMockLogger(logger);
            }
            Options.Logger ??= new WireMockConsoleLogger();
        }

        public MockHttpServerBuilder(string routePrefix, int? port, ILogger logger) : this(port, logger) {
            Options.RoutePrefix = routePrefix;
        }

        public IMockHttpServerBuilder AddModule<T>() where T : IMockHttpMock, new() {
            Options.Mocks.Add(new T());
            return this;
        }

        public IMockHttpServerBuilder AddModule<T>(T instance) where T : IMockHttpMock {
            Options.Mocks.Add(instance);
            return this;
        }

        public MockHttpServer Build() {
            if (hostBuilt) {
                throw new InvalidOperationException("Build already called");
            }
            hostBuilt = true;

            var server = new MockHttpServer(Options)
                .WaitForStart();

            return server;
        }

        public MockHttpServerOptions Options { get; }
    }
}
