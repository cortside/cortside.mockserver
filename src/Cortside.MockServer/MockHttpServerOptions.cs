using System.Collections.Generic;
using WireMock.Logging;
using WireMock.Settings;
using WireMock.Types;

namespace Cortside.MockServer {
    public class MockHttpServerOptions {
        public MockHttpServerOptions() {
            WireMockServerSettings = new WireMockServerSettings {
                CorsPolicyOptions = CorsPolicyOptions.AllowAll,
                StartAdminInterface = true,
                AllowCSharpCodeMatcher = true,
                ReadStaticMappings = true
            };
        }

        public int? Port {
            get => WireMockServerSettings.Port;
            set => WireMockServerSettings.Port = value;
        }
        public IWireMockLogger Logger {
            get => WireMockServerSettings.Logger;
            set => WireMockServerSettings.Logger = value;
        }

        public string RoutePrefix { get; set; }

        public IList<IMockHttpMock> Mocks { get; set; } = new List<IMockHttpMock>();

        internal WireMockServerSettings WireMockServerSettings { get; }
    }
}
