// Mastermind.Wpf/MainViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mastermind.Wpf.Infrastructure;
using Mastermind.Core.Persistence;

namespace Mastermind.Wpf;

public sealed class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnChanged([CallerMemberName] string? n = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

    public object? CurrentView { get; private set; }

    public RelayCommand NytSpilCmd   { get; }
    public RelayCommand OptionsCmd   { get; }
    public RelayCommand StatistikCmd { get; }
    public RelayCommand ExitCmd      { get; }

    private readonly OptionsRepository _optRepo = new();
private readonly IStatistikStore _statsStore = new JsonStatistikStore();

    public MainViewModel()
    {
        NytSpilCmd   = new RelayCommand(() => { CurrentView = new Views.GameView(_optRepo, _statsStore); OnChanged(nameof(CurrentView)); });
    OptionsCmd   = new RelayCommand(() => { CurrentView = new Views.OptionsView(_optRepo);           OnChanged(nameof(CurrentView)); });
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
