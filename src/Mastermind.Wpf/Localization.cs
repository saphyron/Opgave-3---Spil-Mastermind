using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Mastermind.Core.Domain;

namespace Mastermind.Wpf;
/// <summary>
/// Skifter app’ens sprog (da/en) og henter tekster med nøgler.
/// </summary>
/// <remarks>
/// Indsætter en <see cref="ResourceDictionary"/> (Strings.da/en.xaml) i
/// <see cref="Application.Resources.MergedDictionaries"/> og fjerner tidligere sprog-ordbog.
/// Opdaterer også <see cref="Thread.CurrentThread"/>.<see cref="Thread.CurrentUICulture"/>
/// til henholdsvis <c>da-DK</c> eller <c>en-US</c>. Opslagsmetoden bruger
/// <see cref="Application.TryFindResource(object)"/> og <see cref="string.Format(string, object[])"/>.
/// </remarks>
public static class Localization
{
    private static ResourceDictionary? _currentLangDict;
    /// <summary>
    /// Skifter sprog i UI’et (indsætter korrekt tekst-ressource og sætter kultur).
    /// </summary>
    /// <param name="sprog">Ønsket sprog (dansk/engelsk).</param>
    /// <remarks>
    /// Indsætter en <see cref="ResourceDictionary"/> (Strings.da/en.xaml) i
    /// <see cref="Application.Resources.MergedDictionaries"/> og fjerner tidligere sprog-ordbog.
    /// Opdaterer også <see cref="Thread.CurrentThread"/>.<see cref="Thread.CurrentUICulture"/>
    /// til henholdsvis <c>da-DK</c> eller <c>en-US</c>.
    /// </remarks>
    public static void SetLanguage(Sprog sprog)
    {
        var app = Application.Current;
        if (app == null) return;

        var source = sprog == Sprog.Da
            ? "Resources/Strings.da.xaml"
            : "Resources/Strings.en.xaml";

        var dict = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };

        var merged = app.Resources.MergedDictionaries;
        // fjern tidligere sprog-ordbog (første der matcher "Resources/Strings.")
        var existing = merged.FirstOrDefault(d => d.Source != null &&
                                                  d.Source.OriginalString.Contains("Resources/Strings."));
        if (existing != null) merged.Remove(existing);

        merged.Add(dict);
        _currentLangDict = dict;

        // (valgfrit) sæt trådens UI-culture
        var culture = sprog == Sprog.Da ? "da-DK" : "en-US";
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
    }

    /// <summary>
    /// Slår en tekstnøgle op og formatere med argumenter (fallback = nøglen).
    /// </summary>
    /// <param name="key">Ressourcenøgle i sprogfilen.</param>
    /// <param name="args">Valgfrie format-argumenter.</param>
    /// <returns>Den lokaliserede tekst eller nøglen, hvis ikke fundet.</returns>
    /// <remarks>
    /// Bruger <see cref="Application.TryFindResource(object)"/> til opslag i den
    /// aktuelle <see cref="ResourceDictionary"/> (eller forældre).
    /// Hvis fundet og <paramref name="args"/> er angivet, bruges
    /// <see cref="string.Format(string, object[])"/> til at formatere teksten.
    /// Hvis ikke fundet, returneres <paramref name="key"/> uændret.
    /// </remarks>
    public static string T(string key, params object[] args)
    {
        var s = Application.Current.TryFindResource(key) as string ?? key;
        return args is { Length: > 0 } ? string.Format(s, args) : s;
    }
}
