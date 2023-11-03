using System;
using Cortside.MockServer.Logging;
using Microsoft.Extensions.Logging;
using WireMock.Logging;

namespace Cortside.MockServer.Builder {
    public class MockHttpServerBuilder : IMockHttpServerBuilder {
        private bool hostBuilt;

        public MockHttpServerBuilder(MockHttpServerOptions options, ILogger logger = null) {
            Options = options;
            if (logger != null) {
                Options.Logger = new WireMockLogger(logger);
            }
            Options.Logger ??= new WireMockConsoleLogger();
        }

        public MockHttpServerBuilder(int? port, ILogger logger) : this(new MockHttpServerOptions() { Port = port }, logger) {
        }

        public MockHttpServerBuilder(string routePrefix, int? port, ILogger logger) : this(new MockHttpServerOptions() { Port = port, RoutePrefix = routePrefix }, logger) {
        }

        public IMockHttpServerBuilder AddMock<T>() where T : IMockHttpMock, new() {
            Options.Mocks.Add(new T());
            return this;
        }

        public IMockHttpServerBuilder AddMock<T>(T instance) where T : IMockHttpMock {
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
