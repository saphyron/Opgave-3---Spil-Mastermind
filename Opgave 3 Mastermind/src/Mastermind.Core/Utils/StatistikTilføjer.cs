using System.Globalization;
using Mastermind.Core.Domain;

namespace Mastermind.Core.Utils;

public sealed class StatistikTilføjer
{
    public int Runder { get; init; }
    public int Vundne { get; init; }
    public int Tabte  { get; init; }
    public double Sejrsrate { get; init; }          // 0..1
    public double GnsForsøgVedSejr { get; init; }   // 0 hvis ingen sejre
    public int? BedsteSejrForsøg { get; init; }
    public int? VærsteSejrForsøg { get; init; }
    public int BedsteWinStreak { get; init; }
    public int AktuelWinStreak { get; init; }
    public int AktuelLoseStreak { get; init; }

    public static StatistikTilføjer From(IEnumerable<GameResult> results)
    {
        var list = results.OrderBy(r => r.TimestampUtc).ToList();
        int runder = list.Count;
        int vundne = list.Count(r => r.Vundet);
        int tabte  = runder - vundne;

        double gns = 0;
        int? min = null, max = null;
        if (vundne > 0)
        {
            var wins = list.Where(r => r.Vundet).Select(r => r.Forsøg).ToArray();
            gns = wins.Average();
            min = wins.Min();
            max = wins.Max();
        }

        // streaks
        int bestStreak = 0, curWin = 0, curLose = 0;
        foreach (var r in list)
        {
            if (r.Vundet)
            {
                curWin++;
                curLose = 0;
                bestStreak = Math.Max(bestStreak, curWin);
            }
            else
            {
                curLose++;
                curWin = 0;
            }
        }

        return new StatistikTilføjer
        {
            Runder = runder,
            Vundne = vundne,
            Tabte = tabte,
            Sejrsrate = runder > 0 ? (double)vundne / runder : 0,
            GnsForsøgVedSejr = vundne > 0 ? gns : 0,
            BedsteSejrForsøg = min,
            VærsteSejrForsøg = max,
            BedsteWinStreak = bestStreak,
            AktuelWinStreak = curWin,
            AktuelLoseStreak = curLose
        };
    }

    public string RenderTable(string title, CultureInfo ci)
    {
        var rows = new (string Label, string Value)[]
        {
            ("Runder / Rounds", Runder.ToString(ci)),
            ("Sejr / Wins", Vundne.ToString(ci)),
            ("Tab / Losses", Tabte.ToString(ci)),
            ("Win rate", (Sejrsrate*100).ToString("0.0", ci) + "%"),
            ("Gennemsnitlig forsøg / Avg tries", Vundne>0 ? GnsForsøgVedSejr.ToString("0.0", ci) : "-"),
            ("Færest Forsøg / Best win", BedsteSejrForsøg?.ToString(ci) ?? "-"),
            ("Flest Forsøg / Worst win", VærsteSejrForsøg?.ToString(ci) ?? "-"),
            ("Bedste streak / Best win streak", BedsteWinStreak.ToString(ci)),
            ("Nuvær. win streak / Current win streak", AktuelWinStreak.ToString(ci)),
            ("Nuvær. lose streak / Current lose streak", AktuelLoseStreak.ToString(ci)),
        };

        int left = rows.Max(r => r.Label.Length);
        int right= rows.Max(r => r.Value.Length);
        int inner = left + 3 + right;
        string top = "+" + new string('-', inner + 2) + "+";
        string titleLine = "|" + (" " + title + " ").PadLeft(((inner+2)+title.Length)/2).PadRight(inner+2) + "|";
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(top);
        sb.AppendLine(titleLine);
        sb.AppendLine(top);
        foreach (var (l,v) in rows)
            sb.AppendLine("| " + l.PadRight(left) + " │ " + v.PadLeft(right) + " |");
        sb.AppendLine(top);
        return sb.ToString();
    }
}
