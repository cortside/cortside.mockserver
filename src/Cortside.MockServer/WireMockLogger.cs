using System;
using System.Text.Json;
using Serilog;
using WireMock.Admin.Requests;
using WireMock.Logging;

namespace Cortside.MockServer {
    public class WireMockLogger : IWireMockLogger {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions {
            WriteIndented = true
        };

        private readonly ILogger logger;

        public WireMockLogger(ILogger logger) {
            this.logger = logger;
        }

        public void Debug(string formatString, params object[] args) {
            logger.Debug(formatString, args);
        }

        public void Info(string formatString, params object[] args) {
            logger.Information(formatString, args);
        }

        public void Warn(string formatString, params object[] args) {
            logger.Warning(formatString, args);
        }

        public void Error(string formatString, params object[] args) {
            logger.Error(formatString, args);
        }

        public void Error(string formatString, Exception exception) {
            logger.Error(formatString, exception);
        }

        public void DebugRequestResponse(LogEntryModel logEntryModel, bool isAdminRequest) {
            string message = JsonSerializer.Serialize(logEntryModel, options);

            logger.Debug("Admin[{IsAdmin}] {Message}", isAdminRequest, message);
        }
    }
}
