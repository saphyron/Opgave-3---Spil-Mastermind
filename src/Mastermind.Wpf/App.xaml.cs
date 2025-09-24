using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Mastermind.Core.Persistence;

namespace Mastermind.Wpf
{
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += (s, e) =>
            {
                Log("DispatcherUnhandledException", e.Exception);
                MessageBox.Show(e.Exception.ToString(), "UI error");
                e.Handled = true; // så app ikke dør med det samme
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Log("UnhandledException", e.ExceptionObject as Exception);
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Log("UnobservedTaskException", e.Exception);
                e.SetObserved();
            };
        }

        private static void Log(string kind, Exception? ex)
        {
            try
            {
                JsonFilePaths.EnsureDir();
                var logPath = Path.Combine(JsonFilePaths.DatabaseDir, "log.txt");
                File.AppendAllText(logPath,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {kind}: {ex}\r\n");
            }
            catch { /* ignore */ }
        }
    }
}
