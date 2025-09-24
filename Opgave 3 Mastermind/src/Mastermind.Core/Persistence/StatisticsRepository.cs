using System.Text.Json;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence;

public sealed class StatisticsRepository
{
    private static readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true
    };

    public IReadOnlyList<GameResult> LoadAll()
    {
        JsonFilePaths.EnsureDir();
        var path = JsonFilePaths.StatistikPath;
        if (!File.Exists(path)) return Array.Empty<GameResult>();

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
    }

    public void Append(GameResult result)
    {
        JsonFilePaths.EnsureDir();
        var list = LoadAll().ToList();
        list.Add(result);
        var json = JsonSerializer.Serialize(list, _json);
        File.WriteAllText(JsonFilePaths.StatistikPath, json);
    }

    public void Reset()
    {
        JsonFilePaths.EnsureDir();
        File.WriteAllText(JsonFilePaths.StatistikPath, "[]");
    }
}
