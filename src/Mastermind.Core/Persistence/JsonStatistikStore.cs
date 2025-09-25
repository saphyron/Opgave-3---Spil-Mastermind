using System.Text.Json;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence
{
    /// <summary>
    /// Vedvarende lager for statistik (flere GameResult)
    /// </summary>
    /// <remarks>
    /// Implementeres som fil-lager i StatistikStore.
    /// </remarks>
    public sealed class JsonStatistikStore : IStatistikStore
    {
        private static readonly JsonSerializerOptions _json = new() { WriteIndented = true };

        public JsonStatistikStore()
        {
            JsonFilePaths.EnsureDir();
            if (!File.Exists(JsonFilePaths.StatistikPath))
                File.WriteAllText(JsonFilePaths.StatistikPath, "[]");
        }

        public IReadOnlyList<GameResult> LoadAll()
        {
            JsonFilePaths.EnsureDir();
            var path = JsonFilePaths.StatistikPath;

            if (!File.Exists(path)) return new List<GameResult>();
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new List<GameResult>();

            try
            {
                return JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
            }
            catch (JsonException)
            {
                // Korrupt? Gem backup og nulstil
                try { File.Copy(path, path + ".bak", overwrite: true); } catch { }
                File.WriteAllText(path, "[]");
                return new List<GameResult>();
            }
        }

        public void Append(GameResult result)
        {
            var list = LoadAll().ToList();
            list.Add(result);
            var json = JsonSerializer.Serialize(list, _json);
            File.WriteAllText(JsonFilePaths.StatistikPath, json);
        }

        public void Reset() => File.WriteAllText(JsonFilePaths.StatistikPath, "[]");
    }
}
