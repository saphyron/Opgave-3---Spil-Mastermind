using System.Windows.Input;

namespace Mastermind.Wpf.Infrastructure;
/// <summary>
/// En simpel <see cref="ICommand"/>-implementering til WPF/MVVM, der
/// pakker en udførselsdelegat og en valgfri betingelse for, om kommandoen kan køres.
/// </summary>
/// <remarks>
/// Kan bruges direkte i code-behind, eller i en ViewModel.
/// </remarks>
public sealed class RelayCommand : ICommand
{
    private readonly Action _exec;
    private readonly Func<bool>? _can;
    /// <summary>
    /// Opret en ny instans af <see cref="RelayCommand"/>.
    /// </summary>
    /// <param name="exec"></param>
    /// <param name="can"></param>
    /// <remarks>
    /// <paramref name="exec"/> må ikke være null.
    /// <paramref name="can"/> kan være null (i så fald kan kommandoen altid køres).
    /// </remarks>
    public RelayCommand(Action exec, Func<bool>? can = null)
    { _exec = exec; _can = can; }
    /// <summary>
    /// Kan kommandoen køres?
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns>true hvis kommandoen kan udføres; ellers false.</returns>
    /// <remarks>
    /// Parameteret ignoreres.
    /// </remarks>
    public bool CanExecute(object? parameter) => _can?.Invoke() ?? true;
    /// <summary>
    /// Udfører kommandoens handling.
    /// </summary>
    /// <param name="parameter">Ignoreres.</param>
    public void Execute(object? parameter) => _exec();

    public event EventHandler? CanExecuteChanged;
    /// <summary>
    /// Underret WPF om, at CanExecute-værdien er ændret.
    /// </summary>
    /// <remarks>
    /// Skal kaldes, når den underliggende tilstand, som <see cref="CanExecute(object?)"/>
    /// er baseret på, ændres.
    /// </remarks>
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
