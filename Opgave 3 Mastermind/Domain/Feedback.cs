namespace Opgave_3_Mastermind.Domain;

public readonly record struct Feedback(int Black, int White)
{
    public override string ToString() => $"Sort: {Black} | Hvid: {White}";
}
