using System.IO;

namespace Mastermind.Core.Persistence;

public static class JsonFilePaths
{
    /// <summary>
    /// Finder en fælles datamappe:
    /// 1) MASTERMIND_DATA_DIR (env var) hvis sat
    /// 2) Hvis der findes en 'Database' mappe i en forælder af BaseDirectory (dev), brug den
    /// 3) Ellers %LOCALAPPDATA%\Mastermind\Database
    /// </summary>
    public static string DatabaseDir
    {
        get
        {
            // 1) Env override
            var env = Environment.GetEnvironmentVariable("MASTERMIND_DATA_DIR");
            if (!string.IsNullOrWhiteSpace(env))
                return Ensure(env);

            // 2) Dev-fallback: gå opad og find en Database-mappe
            var dir = AppContext.BaseDirectory;
            for (int i = 0; i < 6; i++)
            {
                var candidate = Path.Combine(dir, "Database");
                if (Directory.Exists(candidate))
                    return Ensure(candidate);

                var parent = Directory.GetParent(dir);
                if (parent is null) break;
                dir = parent.FullName;
            }

            // 3) Stabil default: %LOCALAPPDATA%\Mastermind\Database
            var local = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Mastermind", "Database");
            return Ensure(local);

            static string Ensure(string p)
            {
                Directory.CreateDirectory(p);
                return p;
            }
        }
    }

    public static string OptionsPath   => Path.Combine(DatabaseDir, "Options.json");
    public static string StatistikPath => Path.Combine(DatabaseDir, "Statistik.json");

    public static void EnsureDir() => Directory.CreateDirectory(DatabaseDir);
}
