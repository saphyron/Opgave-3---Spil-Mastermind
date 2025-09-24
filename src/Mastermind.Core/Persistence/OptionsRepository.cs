using System.Text.Json;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence;

public sealed class OptionsRepository
{
    private static readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true
    };

    public Options LoadOrDefault()
    {
        JsonFilePaths.EnsureDir();
        var path = JsonFilePaths.OptionsPath;
        if (!File.Exists(path)) return new Options();

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Options>(json) ?? new Options();
    }

    public void Save(Options options)
    {
        JsonFilePaths.EnsureDir();
        var json = JsonSerializer.Serialize(options, _json);
        File.WriteAllText(JsonFilePaths.OptionsPath, json);
    }
}
