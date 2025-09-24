using Mastermind.Core.Domain;
using System.Globalization;

namespace Mastermind.Konsol.Utils
{
    /// <summary>
    /// Håndterer statistik for Mastermind-spillet, herunder antal spil, sejre, tab, forsøg og win-streaks.
    /// </summary>
    /// <remarks>
    /// Statistikken kan registrere resultater af spil og vise en oversigt baseret på de angivne Options.
    /// Denne klasse bruger Options til at tilpasse visningen baseret på spillets indstillinger, såsom sprog.
    /// Derudover beregnes statistik som gennemsnitlige forsøg ved sejr, bedste og værste forsøg, samt win-rate.
    /// </remarks>
    public class Statistik
    {
        // -------- Statistikdata -------- //
        // ------------------------------ for antal spil, sejre og tab
        public int AntalSpil { get; private set; }
        public int AntalSejre { get; private set; }
        public int AntalTab { get; private set; }
        // ------------------------------ Sejrs-forsøg
        public int SumForsøgVedSejr { get; private set; }
        public double GnsForsøgVedSejr => AntalSejre == 0 ? 0 : (double)SumForsøgVedSejr / AntalSejre;
        public int? BedsteForsøgVedSejr { get; private set; }
        public int? BedsteForsøgVedTab { get; private set; }
        public int? VærsteForsøgVedSejr { get; private set; }

        // ------------------------------ Streaks
        public int NuværendeStreak { get; private set; }
        public int BedsteStreak { get; private set; }
        public double WinRate => AntalSpil == 0 ? 0 : (double)AntalSejre / AntalSpil * 100;
        /// <summary>
        /// Registrerer en sejr i statistikken med det givne antal forsøg.
        /// </summary>
        /// <param name="forsøg"></param>
        /// <remarks>
        /// Opdaterer statistikdata såsom antal spil, sejre, forsøg ved sejr, og win-streaks.
        /// Sørger også for at opdatere bedste og værste forsøg ved sejr.
        /// </remarks>
        public void RegisterSejr(int forsøg)
        {
            AntalSpil++;
            AntalSejre++;
            SumForsøgVedSejr += forsøg;
            if (BedsteForsøgVedSejr == null || forsøg < BedsteForsøgVedSejr)
                BedsteForsøgVedSejr = forsøg;
            if (VærsteForsøgVedSejr == null || forsøg > VærsteForsøgVedSejr)
                VærsteForsøgVedSejr = forsøg;

            NuværendeStreak++;
            if (NuværendeStreak > BedsteStreak)
                BedsteStreak = NuværendeStreak;
        }
        /// <summary>
        /// Registrerer et tab i statistikken.
        /// </summary>
        /// <remarks>
        /// Opdaterer statistikdata såsom antal spil, tab, og nulstiller den nuværende win-streak.
        /// </remarks>
        public void RegisterTab()
        {
            AntalSpil++;
            AntalTab++;
            NuværendeStreak = 0; // Reset vinder-streak
        }
        /// <summary>
        /// Viser en oversigt over statistikken i konsollen baseret på de angivne Options.
        /// </summary>
        /// <param name="options"></param>
        /// <remarks>
        /// Denne metode viser statistikdata såsom antal spil, sejre, tab, win-rate,
        /// gennemsnitlige forsøg ved sejr, bedste og værste forsøg ved sejr.
        /// Layoutet er i ren ASCII-stil og tilpasses baseret på det valgte sprog i Options.
        /// Derudover håndteres sprogvalg (dansk/engelsk) for labels i oversigten.
        /// </remarks>
        public void Render(Options options)
        {
            var lang = options.sprog;
            var ci = lang == Sprog.Da ? CultureInfo.GetCultureInfo("da-DK") : CultureInfo.InvariantCulture;

            // Rækker (label, værdi)
            var rows = new List<(string Label, string Value)>
            {
                (L("Rounds", lang),        AntalSpil.ToString(ci)),
                (L("Wins", lang),          AntalSejre.ToString(ci)),
                (L("Losses", lang),        AntalTab.ToString(ci)),
                (L("WinRate", lang),       WinRate.ToString("0.0", ci) + "%"),
                (L("AvgTriesWin", lang),   AntalSejre > 0 ? GnsForsøgVedSejr.ToString("0.0", ci) : "-"),
                (L("BestWin", lang),       BedsteForsøgVedSejr?.ToString(ci) ?? "-"),
                (L("WorstWin", lang),      VærsteForsøgVedSejr?.ToString(ci) ?? "-"),
                (L("BestStreak", lang),    BedsteStreak.ToString(ci)),
                (L("CurWinStreak", lang),  NuværendeStreak.ToString(ci)),
            };

            // Layout (ren ASCII – “MS-DOS” stil)
            int leftWidth = 0;
            int rightWidth = 0;
            foreach (var (l, v) in rows)
            {
                leftWidth = Math.Max(leftWidth, l.Length);
                rightWidth = Math.Max(rightWidth, v.Length);
            }

            int padLeft = leftWidth;
            int padRight = rightWidth;
            int inner = padLeft + 3 + padRight; // "label │ value"
            string top = "+" + new string('-', inner + 2) + "+"; // 2 for lodrette margener
            string sep = "+" + new string('-', inner + 2) + "+";
            string btm = top;

            Console.WriteLine();
            Console.WriteLine(top);
            // Titel centreret
            var title = " " + L("Title", lang) + " ";
            var titleLine = "|" + title.PadLeft(((inner + 2) + title.Length) / 2).PadRight(inner + 2) + "|";
            Console.WriteLine(titleLine);
            Console.WriteLine(sep);

            foreach (var (label, value) in rows)
            {
                string line = "| " +
                              label.PadRight(padLeft) +
                              " │ " +
                              value.PadLeft(padRight) +
                              " |";
                Console.WriteLine(line);
            }

            Console.WriteLine(btm);
            Console.WriteLine();
        }
        /// <summary>
        /// Lokaliserer en streng baseret på det angivne sprog.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s"></param>
        /// <returns>Den lokaliserede streng.</returns>
        /// <remarks>
        /// Denne metode håndterer oversættelse af nøgler til den korrekte sprogversion.
        /// </remarks>
        private static string L(string key, Sprog s) => s == Sprog.Da ? Da(key) : En(key);
        /// <summary>
        /// Danske lokaliserede strenge.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Den lokaliserede danske streng.</returns>
        /// <remarks>
        /// Denne metode håndterer oversættelse af nøgler til den danske sprogversion.
        /// </remarks>
        private static string Da(string key) => key switch
        {
            "Title"         => "STATISTIK (SESSION)",
            "Rounds"        => "Runder",
            "Wins"          => "Vundne",
            "Losses"        => "Tabte",
            "WinRate"       => "Sejrsrate",
            "AvgTriesWin"   => "Gns. forsøg (sejr)",
            "BestWin"       => "Bedste sejr (få)",
            "WorstWin"      => "Værste sejr (flest)",
            "BestStreak"    => "Bedste win-streak",
            "CurWinStreak"  => "Aktuel win-streak",
            _ => key
        };
        /// <summary>
        /// Engelske lokaliserede strenge.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Den lokaliserede engelske streng.</returns>
        /// <remarks>
        /// Denne metode håndterer oversættelse af nøgler til den engelske sprogversion.
        /// </remarks>
        private static string En(string key) => key switch
        {
            "Title"         => "STATISTICS (SESSION)",
            "Rounds"        => "Rounds",
            "Wins"          => "Wins",
            "Losses"        => "Losses",
            "WinRate"       => "Win rate",
            "AvgTriesWin"   => "Avg tries (win)",
            "BestWin"       => "Best win (fewest)",
            "WorstWin"      => "Worst win (most)",
            "BestStreak"    => "Best win streak",
            "CurWinStreak"  => "Current win streak",
            _ => key
        };
    }
}