using System;
using System.Windows.Input;

namespace Mastermind.Wpf.Infrastructure;

public sealed class RelayCommand : ICommand
{
    private readonly Action _exec;
    private readonly Func<bool>? _can;

    public RelayCommand(Action exec, Func<bool>? can = null)
    { _exec = exec; _can = can; }

    public bool CanExecute(object? parameter) => _can?.Invoke() ?? true;
    public void Execute(object? parameter) => _exec();

    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
