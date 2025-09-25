using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Mastermind.Core.Domain;

namespace Mastermind.Wpf;

public static class Localization
{
    private static ResourceDictionary? _currentLangDict;

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

    // Lille helper til code-behind-tekster:
    public static string T(string key, params object[] args)
    {
        var s = Application.Current.TryFindResource(key) as string ?? key;
        return args is { Length: > 0 } ? string.Format(s, args) : s;
    }
}
