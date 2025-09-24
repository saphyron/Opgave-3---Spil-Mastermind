using System.Text.Json;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence
{
    public sealed class JsonStatistikStore : IStatistikStore
    {
        private readonly string _dir;
        private readonly string _path;
        private static readonly JsonSerializerOptions _json = new() { WriteIndented = true };

        public JsonStatistikStore(string? baseDir = null)
        {
            // Gem i ./database/Statistik.json ved runtime
            _dir  = Path.Combine(baseDir ?? AppContext.BaseDirectory, "database");
            _path = Path.Combine(_dir, "Statistik.json");
            Directory.CreateDirectory(_dir);
            if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
        }

        public IReadOnlyList<GameResult> LoadAll()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
        }

        public void Append(GameResult result)
        {
            var list = LoadAll().ToList();
            list.Add(result);
            var json = JsonSerializer.Serialize(list, _json);
            File.WriteAllText(_path, json);
        }

        public void Reset() => File.WriteAllText(_path, "[]");
    }
}
