using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortside.MockServer {
    public static class MockHttpServerExtensions {

        /// <summary>
        /// End/Cancel when the user hits Ctrl+C.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sleepTime"></param>
        /// <returns></returns>
        public static Task WaitForCancelKeyPressAsync(this MockHttpServer server, int sleepTime = 30000) {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => {
                server.Logger.Info("Canceling for CancelKeyPress");
                cts.Cancel();
                e.Cancel = true;
            };

            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += ctx => {
                if (!cts.IsCancellationRequested) {
                    server.Logger.Info("Canceling for AssemblyLoadContext.Default.Unloading");
                    cts.Cancel();
                }
            };

            return ReportStatusAsync(server, sleepTime, cts.Token);
        }

        private static Task ReportStatusAsync(MockHttpServer server, int sleepTime, CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                server.Logger.Info($"MockHttpServer server running : {server.IsStarted}");
                var cancellationTriggered = cancellationToken.WaitHandle.WaitOne(sleepTime);
                if (cancellationTriggered) {
                    server.Logger.Info($"WaitOne exited, cancel triggered {cancellationTriggered}, cancel requested {cancellationToken.IsCancellationRequested}");
                }
            }

            return Task.CompletedTask;
        }
    }
}
