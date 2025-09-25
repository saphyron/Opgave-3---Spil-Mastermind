using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Persistence;
using Mastermind.Core.Services;
using Mastermind.Core.Utils; // for StatistikTilføjer
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
    /// <summary>
    /// Brugerflade til Mastermind-spillet, hvor spilleren kan gætte farver og starte nye runder.
    /// </summary>
    /// <remarks>
    /// Implementerer WPF <see cref="UserControl"/> og styrer spildata, evaluering af gæt og 
    /// opdatering af brugerfladen. Arbejder sammen med <see cref="OptionsRepository"/>, 
    /// <see cref="SecretGenerator"/>, <see cref="Evaluering"/> og <see cref="IStatistikStore"/>.
    /// </remarks>
    public partial class GameView : UserControl
    {
        public ObservableCollection<Farve> Palette { get; } =
            new ObservableCollection<Farve>(Enum.GetValues<Farve>());

        public Farve Guess0 { get; set; }
        public Farve Guess1 { get; set; }
        public Farve Guess2 { get; set; }
        public Farve Guess3 { get; set; }

        public ObservableCollection<string> History { get; } = new();
        public string Footer { get; private set; } = "";

        private readonly OptionsRepository _optRepo;
        private readonly IStatistikStore _stats;

        private Options _opt;
        private Farve[] _secret = Array.Empty<Farve>();
        private readonly SecretGenerator _generator;
        private readonly Evaluering _eval;

        public RelayCommand GaetCmd { get; }
        public RelayCommand NyRundeCmd { get; }

        private int _attempts;
        private bool _roundOver;
        /// <summary>
        /// Initialiserer en ny spilvisning og starter første runde.
        /// </summary>
        /// <param name="optRepo">Repository til indstillinger.</param>
        /// <param name="stats">Vedvarende lager til statistik.</param>
        /// <remarks>
        /// Indstillinger indlæses ved start, og en ny hemmelig kode genereres.
        /// Kommandoer til at gætte og starte ny runde oprettes.
        /// </remarks>
        public GameView(OptionsRepository optRepo, IStatistikStore stats)
        {
            InitializeComponent();
            DataContext = this;

            _optRepo = optRepo;
            _stats = stats;

            _opt = _optRepo.LoadOrDefault();
            _generator = new SecretGenerator(_opt);
            _eval = new Evaluering();

            GaetCmd = new RelayCommand(HandleGuess, () => !_roundOver);
            NyRundeCmd = new RelayCommand(StartNewRound);

            StartNewRound();
        }
        /// <summary>
        /// Starter en ny runde og nulstiller hemmelig kode, historik og forsøgstæller.
        /// </summary>
        /// <remarks>
        /// Indstillinger genindlæses, og brugerfladen opdateres.
        /// </remarks>
        private void StartNewRound()
        {
            _opt = _optRepo.LoadOrDefault();
            _secret = _generator.GenerateSecret();
            _attempts = 0;
            _roundOver = false;
            History.Clear();
            Footer = _opt.sprog == Sprog.Da ? "Ny runde startet." : "New round started.";
            GaetCmd.RaiseCanExecuteChanged();
            DataContext = null; DataContext = this;
        }
        /// <summary>
        /// Håndterer et gæt og evaluerer om spilleren har vundet, tabt eller kan fortsætte.
        /// </summary>
        /// <remarks>
        /// Opdaterer historik, statistik og brugerflade baseret på resultatet af gættet.
        /// </remarks>
        private void HandleGuess()
        {
            if (_roundOver) return;

            var guess = new[] { Guess0, Guess1, Guess2, Guess3 };
            _attempts++;

            var fb = _eval.Evaluer(guess, _secret);
            History.Add(_opt.sprog == Sprog.Da
                ? $"{_attempts}: Sort: {fb.Black} | Hvid: {fb.White}"
                : $"{_attempts}: Black: {fb.Black} | White: {fb.White}");

            // vundet?
            if (fb.Black == _opt.længde)
            {
                _stats.Append(new GameResult(DateTime.UtcNow, true, _attempts));
                _roundOver = true;
                GaetCmd.RaiseCanExecuteChanged();

                Footer = BuildFooterSummary(won: true, showSecret: false);
                DataContext = null; DataContext = this;
                return; // ikke auto-start ny runde
            }

            // tabt?
            if (_attempts >= _opt.maxForsøg)
            {
                _stats.Append(new GameResult(DateTime.UtcNow, false, _attempts));
                _roundOver = true;
                GaetCmd.RaiseCanExecuteChanged();

                Footer = BuildFooterSummary(won: false, showSecret: true);
                DataContext = null; DataContext = this;
                return; // ikke auto-start ny runde
            }

            Footer = _opt.sprog == Sprog.Da ? "Fortsæt med at gætte..." : "Keep guessing...";
            DataContext = null; DataContext = this;
        }
        /// <summary>
        /// Bygger en opsummering til fodnoten med resultat og statistik.
        /// </summary>
        /// <param name="won">Om spilleren har vundet.</param>
        /// <param name="showSecret">Om den hemmelige kode skal vises.</param
        /// <returns>Opsummeringstekst.</returns>
        /// <remarks>
        /// Inkluderer resultatlinje, hemmelig kode (hvis relevant), samlet statistik og
        /// instruktion om at starte ny runde.
        /// </remarks>
        private string BuildFooterSummary(bool won, bool showSecret)
        {
            var list = _stats.LoadAll();
            var agg = StatistikTilføjer.From(list);

            string lineResult = won
                ? (_opt.sprog == Sprog.Da ? $"Du vandt på {_attempts} forsøg." : $"You won in {_attempts} tries.")
                : (_opt.sprog == Sprog.Da ? $"Du tabte efter {_attempts} forsøg."
                                          : $"You lost after {_attempts} tries.");

            string secretLine = "";
            if (showSecret)
            {
                var secretTxt = string.Join(" ",
                    _secret.Select(f => FarverHelper.ToName(f, _opt.sprog)));

                secretLine = _opt.sprog == Sprog.Da
                ? $"Hemmelig kode: {secretTxt}"
                : $"Secret code: {secretTxt}";
            }

            // en kompakt stats-linje
            string statsLine = _opt.sprog == Sprog.Da
                ? $"Runder: {agg.Runder} | Sejr: {agg.Vundne} | Tab: {agg.Tabte} | Win rate: {(agg.Sejrsrate * 100):0.0}% | Gns. forsøg (sejr): {(agg.Vundne > 0 ? agg.GnsForsøgVedSejr.ToString("0.0") : "-")}"
                : $"Rounds: {agg.Runder} | Wins: {agg.Vundne} | Losses: {agg.Tabte} | Win rate: {(agg.Sejrsrate * 100):0.0}% | Avg tries (win): {(agg.Vundne > 0 ? agg.GnsForsøgVedSejr.ToString("0.0") : "-")}";

            string hint = _opt.sprog == Sprog.Da
                ? "Tryk 'Ny runde' for at starte igen."
                : "Press 'New round' to start again.";

            return string.Join(Environment.NewLine,
                new[] { lineResult }
                .Concat(string.IsNullOrEmpty(secretLine) ? Array.Empty<string>() : new[] { secretLine })
                .Concat(new[] { statsLine, hint }));
        }
    }
}
