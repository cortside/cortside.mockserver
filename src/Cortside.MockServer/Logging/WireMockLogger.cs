#pragma warning disable xSerilog004 // MessageTemplate argument formatString is not constant

using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WireMock.Admin.Requests;
using WireMock.Logging;

namespace Cortside.MockServer.Logging {
    public class WireMockLogger : IWireMockLogger {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions {
            WriteIndented = true
        };

        private readonly ILogger logger;

        public WireMockLogger(ILogger logger) {
            this.logger = logger;
        }

        public void Debug(string formatString, params object[] args) {
            logger.LogDebug(formatString, args);
        }

        public void Info(string formatString, params object[] args) {
            logger.LogInformation(formatString, args);
        }

        public void Warn(string formatString, params object[] args) {
            logger.LogWarning(formatString, args);
        }

        public void Error(string formatString, params object[] args) {
            logger.LogError(formatString, args);
        }

        public void Error(string formatString, Exception exception) {
            logger.LogError(formatString, exception);
        }

        public void DebugRequestResponse(LogEntryModel logEntryModel, bool isAdminRequest) {
            string message = JsonSerializer.Serialize(logEntryModel, options);

            logger.LogDebug("Admin[{IsAdmin}] {Message}", isAdminRequest, message);
        }
    }
}
