using System.IO;

namespace Mastermind.Core.Persistence;

public static class JsonFilePaths
{
    public static string DatabaseDir =>
        Path.Combine(AppContext.BaseDirectory, "Database");

    public static string OptionsPath =>
        Path.Combine(DatabaseDir, "Options.json");

    public static string StatistikPath =>
        Path.Combine(DatabaseDir, "Statistik.json");

    public static void EnsureDir() =>
        Directory.CreateDirectory(DatabaseDir);
}
