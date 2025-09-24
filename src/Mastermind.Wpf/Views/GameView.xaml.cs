// Mastermind.Wpf/Views/GameView.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Services;
using Mastermind.Core.Persistence;
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
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
        private readonly IStatistikStore _stats;         // ← interface
        private Options _opt;
        private Farve[] _secret = Array.Empty<Farve>();
        private readonly SecretGenerator _generator;
        private readonly Evaluering _eval;

        public RelayCommand GaetCmd { get; }
        public RelayCommand NyRundeCmd { get; }

        private int _attempts;

        public GameView(OptionsRepository optRepo, IStatistikStore stats)
        {
            InitializeComponent();
            DataContext = this;

            _optRepo = optRepo;
            _stats   = stats;

            _opt = _optRepo.LoadOrDefault();
            _generator = new SecretGenerator(_opt);
            _eval = new Evaluering();

            GaetCmd = new RelayCommand(HandleGuess);
            NyRundeCmd = new RelayCommand(StartNewRound);

            StartNewRound();
        }

        private void StartNewRound()
        {
            _opt = _optRepo.LoadOrDefault();
            _secret = _generator.GenerateSecret();
            _attempts = 0;
            History.Clear();
            Footer = _opt.sprog == Sprog.Da ? "Ny runde startet." : "New round started.";
            DataContext = null; DataContext = this;
        }

        private void HandleGuess()
        {
            var guess = new[] { Guess0, Guess1, Guess2, Guess3 };
            _attempts++;

            var fb = _eval.Evaluer(guess, _secret);
            History.Add(_opt.sprog == Sprog.Da
                ? $"{_attempts}: Sort: {fb.Black} | Hvid: {fb.White}"
                : $"{_attempts}: Black: {fb.Black} | White: {fb.White}");

            if (fb.Black == _opt.længde)
            {
                _stats.Append(new GameResult(DateTime.UtcNow, true, _attempts));
                System.Windows.MessageBox.Show(JsonFilePaths.StatistikPath, "Skrev til");

                Footer = _opt.sprog == Sprog.Da ? "Du vandt! Ny runde startet." : "You won! New round started.";
                StartNewRound();
            }
            else if (_attempts >= _opt.maxForsøg)
            {
                _stats.Append(new GameResult(DateTime.UtcNow, false, _attempts));
                System.Windows.MessageBox.Show(JsonFilePaths.StatistikPath, "Skrev til");

                Footer = _opt.sprog == Sprog.Da ? "Du tabte! Ny runde startet." : "You lost! New round started.";
                StartNewRound();
            }
            else
            {
                Footer = _opt.sprog == Sprog.Da ? "Fortsæt med at gætte..." : "Keep guessing...";
                DataContext = null; DataContext = this;
            }
        }
    }
}
