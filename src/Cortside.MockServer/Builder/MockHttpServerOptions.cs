using System.Collections.Generic;
using WireMock.Logging;
using WireMock.Settings;
using WireMock.Types;

namespace Cortside.MockServer.Builder {
    public class MockHttpServerOptions {
        public MockHttpServerOptions() {
            WireMockServerSettings = new WireMockServerSettings {
                CorsPolicyOptions = WireMock.Types.CorsPolicyOptions.AllowAll,
                StartAdminInterface = false,
                AllowCSharpCodeMatcher = false,
                ReadStaticMappings = false
            };
        }

        public CorsPolicyOptions? CorsPolicyOptions {
            get => WireMockServerSettings.CorsPolicyOptions;
            set => WireMockServerSettings.CorsPolicyOptions = value;
        }


        public bool? StartAdminInterface {
            get => WireMockServerSettings.StartAdminInterface;
            set => WireMockServerSettings.StartAdminInterface = value;
        }

        public bool? AllowCSharpCodeMatcher {
            get => WireMockServerSettings.AllowCSharpCodeMatcher;
            set => WireMockServerSettings.AllowCSharpCodeMatcher = value;
        }

        public bool? ReadStaticMappings {
            get => WireMockServerSettings.ReadStaticMappings;
            set => WireMockServerSettings.ReadStaticMappings = value;
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
