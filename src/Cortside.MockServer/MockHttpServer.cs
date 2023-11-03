using System;
using System.Net.Http;
using Cortside.MockServer.Builder;
using Microsoft.Extensions.Logging;
using WireMock.Logging;
using WireMock.Server;

namespace Cortside.MockServer {
    public class MockHttpServer : IDisposable {
        private readonly WireMockServer server;
        private bool disposed;

        public static IMockHttpServerBuilder CreateBuilder(MockHttpServerOptions options, ILogger logger = null) {
            var builder = new MockHttpServerBuilder(options, logger);
            return builder;
        }

        public static IMockHttpServerBuilder CreateBuilder(int? port = null, ILogger logger = null) {
            var builder = new MockHttpServerBuilder(port, logger);
            return builder;
        }

        public static IMockHttpServerBuilder CreateBuilder(string routePrefix, int? port = null, ILogger logger = null) {
            var builder = new MockHttpServerBuilder(routePrefix, port, logger);
            return builder;
        }

        public MockHttpServer(MockHttpServerOptions options) {
            Logger = options.Logger;

            Logger.Info("Waiting for server to start");
            server = WireMockServer.Start(options.WireMockServerSettings);

            foreach (var mock in options.Mocks) {
                var name = mock.GetType().FullName;
                Logger.Debug($"Starting mock {name}");
                StartMock(mock);
            }

            Logger.Info($"Server is listening at {Url}");
        }

        public IWireMockLogger Logger { get; }

        public WireMockServer WireMockServer => server;

        public string[] Urls => server.Urls;

        public string Url => server.Urls[0];

        public Uri Uri => new Uri(Url);

        public string RoutePrefix { get; }

        public string RoutePrefixUrl => $"{Url}/{RoutePrefix}";

        public bool IsStarted { get; internal set; }

        internal MockHttpServer WaitForStart() {
            while (!server.IsStarted) {
                // nothing to do here
            }
            IsStarted = true;
            return this;
        }

        public void Stop() {
            server.Stop();
        }

        private void StartMock(IMockHttpMock mock) {
            mock.Configure(this);
        }

        /// <summary>
        /// Creates a new instance of an <see cref="HttpClient"/> that can be used to
        /// send <see cref="HttpRequestMessage"/> to the server. The base address of the <see cref="HttpClient"/>
        /// instance will be set to <c>http://localhost</c>.
        /// </summary>
        /// <param name="handlers">A list of <see cref="DelegatingHandler"/> instances to set up on the
        /// <see cref="HttpClient"/>.</param>
        /// <returns>The <see cref="HttpClient"/>.</returns>
        public HttpClient CreateClient(params DelegatingHandler[] handlers) {
            if (!IsStarted) {
                throw new InvalidOperationException("server not running");
            }

            var client = new HttpClient();
            ConfigureClient(client);

            return client;
        }

        /// <summary>
        /// Creates a new instance of an <see cref="HttpClient"/> that can be used to
        /// send <see cref="HttpRequestMessage"/> to the server.
        /// </summary>
        /// <param name="baseAddress">The base address of the <see cref="HttpClient"/> instance.</param>
        /// <param name="handlers">A list of <see cref="DelegatingHandler"/> instances to set up on the
        /// <see cref="HttpClient"/>.</param>
        /// <returns>The <see cref="HttpClient"/>.</returns>
        public HttpClient CreateDefaultClient(Uri baseAddress, params DelegatingHandler[] handlers) {
            var client = CreateClient(handlers);
            client.BaseAddress = baseAddress;

            return client;
        }

        /// <summary>
        /// Configures <see cref="HttpClient"/> instances created by this <see cref="WebApplicationFactory{TEntryPoint}"/>.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/> instance getting configured.</param>
        protected virtual void ConfigureClient(HttpClient client) {
            if (client == null) {
                throw new ArgumentNullException(nameof(client));
            }

            client.BaseAddress = Uri;
        }

        public void Dispose() {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }

            if (disposing) {
                server?.Stop();
                server?.Dispose();
            }

            disposed = true;
        }
    }
}
