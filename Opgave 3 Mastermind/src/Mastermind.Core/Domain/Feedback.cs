namespace Mastermind.Core.Domain;

/// <summary>
/// Repræsenterer feedback fra en evaluering af et gæt i Mastermind.
/// </summary>
/// <param name="Black"></param>
/// <param name="White"></param>
/// <remarks>
/// Denne record struct indeholder antallet af sorte (Black) og hvide (White) markører i feedbacken.
/// </remarks>
public readonly record struct Feedback(int Black, int White)
{
    public override string ToString() => $"Sort: {Black} | Hvid: {White}";
}
