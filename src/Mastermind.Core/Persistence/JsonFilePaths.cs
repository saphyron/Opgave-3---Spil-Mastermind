using System;
using System.IO;
using System.Linq;

namespace Mastermind.Core.Persistence;

public static class JsonFilePaths
{
    public static string DatabaseDir
    {
        get
        {
            // 1) Eksplicit override (meget stabilt)
            var env = Environment.GetEnvironmentVariable("MASTERMIND_DATA_DIR");
            if (!string.IsNullOrWhiteSpace(env))
                return Ensure(env);

            // 2) Gå op fra BaseDirectory og find løsningens rod (mappe med *.sln),
            //    og brug dens \Database.
            var cur = new DirectoryInfo(AppContext.BaseDirectory);
            for (int depth = 0; cur != null && depth < 15; depth++, cur = cur.Parent)
            {
                var curPath = cur.FullName;
                var curName = cur.Name.ToLowerInvariant();

                // skip build-mapper
                bool isBuild = curName == "bin" || curName == "obj";

                // helst: rod med .sln
                bool hasSln = Directory.EnumerateFiles(curPath, "*.sln", SearchOption.TopDirectoryOnly).Any();
                if (hasSln)
                {
                    var fromSln = Path.Combine(curPath, "Database");
                    if (Directory.Exists(fromSln))
                        return Ensure(fromSln);
                }

                // ellers: brug nærmeste \Database som IKKE ligger i bin/obj
                var dbHere = Path.Combine(curPath, "Database");
                if (Directory.Exists(dbHere) && !isBuild)
                    return Ensure(dbHere);
            }

            // 3) Fallback: pr. bruger
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
