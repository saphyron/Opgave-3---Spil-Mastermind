// Mastermind.Wpf/MainViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mastermind.Wpf.Infrastructure;
using Mastermind.Core.Persistence;

namespace Mastermind.Wpf;
/// <summary>
/// Hoved-viewmodel der skifter mellem visninger (spil, indstillinger, statistik) og håndterer exit.
/// </summary>
/// <remarks>
/// Implementerer <see cref="INotifyPropertyChanged"/> og eksponerer kommandoer til navigation:
/// <c>NytSpilCmd</c>, <c>OptionsCmd</c>, <c>StatistikCmd</c>, <c>ExitCmd</c>.
/// Opretter og genbruger <see cref="OptionsRepository"/> og <see cref="JsonStatistikStore"/> og
/// sætter <c>CurrentView</c> til den aktive WPF-view (UserControl). Ved ændringer kaldes
/// <see cref="OnChanged(string?)"/> for at opdatere bindings.
/// </remarks>
public sealed class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
        /// Udløser <see cref="PropertyChanged"/> for bindings.
        /// </summary>
        /// <param name="n">Navnet på den ændrede property (udfyldes automatisk).</param>
        /// <remarks>
        /// Kaldes når en property ændres for at opdatere UI via bindings.
        /// </remarks>
    void OnChanged([CallerMemberName] string? n = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

    public object? CurrentView { get; private set; }

    public RelayCommand NytSpilCmd   { get; }
    public RelayCommand OptionsCmd   { get; }
    public RelayCommand StatistikCmd { get; }
    public RelayCommand ExitCmd      { get; }

    private readonly OptionsRepository _optRepo = new();
private readonly IStatistikStore _statsStore = new JsonStatistikStore();

/// <summary>
        /// Initialiserer kommandoer, sætter startvisning og håndterer afslutning af app.
        /// </summary>
        /// <remarks>
        /// Opretter kommandoer til at skifte mellem <see cref="Views.GameView"/>,
        /// <see cref="Views.OptionsView"/> og <see cref="Views.StatistikView"/>.
        /// Starter med <see cref="Views.GameView"/> som aktiv visning.
        /// Exit-kommandoen lukker WPF-appen eller, hvis den ikke findes,
        /// kalder <see cref="Environment.Exit(int)"/>.
        /// </remarks>
    public MainViewModel()
    {
        NytSpilCmd = new RelayCommand(() => { CurrentView = new Views.GameView(_optRepo, _statsStore); OnChanged(nameof(CurrentView)); });
        OptionsCmd = new RelayCommand(() => { CurrentView = new Views.OptionsView(_optRepo); OnChanged(nameof(CurrentView)); });
        StatistikCmd = new RelayCommand(() => { CurrentView = new Views.StatistikView(_statsStore, _optRepo); OnChanged(nameof(CurrentView)); });

        ExitCmd = new RelayCommand(() =>
        {
            var app = System.Windows.Application.Current;
            if (app is not null) app.Shutdown();
            else System.Environment.Exit(0);
        });

        CurrentView = new Views.GameView(_optRepo, _statsStore);
        OnChanged(nameof(CurrentView));
    }
}
