using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Mastermind.Core.Persistence;

namespace Mastermind.Wpf
{
    /// <summary>
    /// Starter appen og fanger uventede fejl: viser en besked til brugeren og skriver en logfil.
    /// </summary>
    /// <remarks>
    /// Abonnerer på tre globale fejlhændelser:
    /// <list type="bullet">
    ///   <item><description><see cref="Application.DispatcherUnhandledException"/> for UI-tråden (markeres som håndteret for at undgå crash).</description></item>
    ///   <item><description><see cref="AppDomain.UnhandledException"/> for ikke-UI tråde.</description></item>
    ///   <item><description><see cref="TaskScheduler.UnobservedTaskException"/> for uobserverede task-exceptions (kalder <c>SetObserved()</c>).</description></item>
    /// </list>
    /// Alle fejl skrives appendende til <c>log.txt</c> i <see cref="JsonFilePaths.DatabaseDir"/> efter at <see cref="JsonFilePaths.EnsureDir"/> er kaldt.
    /// </remarks>
    public partial class App : Application
    {
        /// <summary>
        /// Initialiserer globale fejlhandlere og logger fejl ved runtime.
        /// </summary>
        /// <remarks>
        /// Se klassekommentar for detaljer.
        /// </remarks>
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
        /// <summary>
        /// Skriver en loglinje med tidsstempel, fejltype og exception til logfilen.
        /// </summary>
        /// <param name="kind">Kort betegnelse for fejlkilden (fx "UnhandledException").</param>
        /// <param name="ex">Den tilhørende exception (kan være <c>null</c>).</param>
        /// <remarks>
        /// Forsøger at sikre at <see cref="JsonFilePaths.DatabaseDir"/> eksisterer.
        /// Hvis logskrivning fejler, ignoreres fejlen.
        /// </remarks>
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
