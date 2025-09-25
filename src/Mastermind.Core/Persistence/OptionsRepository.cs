using System.Text.Json;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence;
/// <summary>
/// Repository for Options (indstillinger)
/// </summary>
/// <remarks>
/// Gemmes i Options.json i Database-mappen.
/// </remarks>
public sealed class OptionsRepository
{
    private static readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true
    };
    /// <summary>
    /// Læs Options fra fil, eller returner default (og skriv fil hvis ikke eksisterer)
    /// </summary>
    /// <returns>Options</returns>
    /// <remarks>
    /// Hvis filen er tom eller korrupt, overskrives den med default.
    /// </remarks>
    public Options LoadOrDefault()
    {
        JsonFilePaths.EnsureDir();
        var path = JsonFilePaths.OptionsPath;

        // lokal hjælpefunktion til at skrive og returnere defaults
        static Options DefaultsAndSave(string path, JsonSerializerOptions json)
        {
            var def = new Options();          // default
            var txt = JsonSerializer.Serialize(def, json);
            File.WriteAllText(path, txt);
            return def;
        }

        try
        {
            if (!File.Exists(path))
                return DefaultsAndSave(path, _json);

            var json = File.ReadAllText(path);

            // TOM / WHITESPACE → skriv defaults
            if (string.IsNullOrWhiteSpace(json))
                return DefaultsAndSave(path, _json);

            var opt = JsonSerializer.Deserialize<Options>(json);
            return opt ?? DefaultsAndSave(path, _json);
        }
        catch (JsonException)
        {
            // korrupt JSON → tag backup og skriv defaults
            try { File.Copy(path, path + ".bak", overwrite: true); } catch { /* ignorér */ }
            return DefaultsAndSave(path, _json);
        }
        catch
        {
            // sidste udvej: bare giv defaults (uden at skrive)
            return new Options();
        }
    }
    /// <summary>
    /// Gem Options til fil
    /// </summary>
    /// <param name="options"></param>
    /// <remarks>
    /// Overskriver eksisterende fil.
    /// </remarks>
    public void Save(Options options)
    {
        JsonFilePaths.EnsureDir();
        var txt = JsonSerializer.Serialize(options, _json);
        File.WriteAllText(JsonFilePaths.OptionsPath, txt);
    }
}
