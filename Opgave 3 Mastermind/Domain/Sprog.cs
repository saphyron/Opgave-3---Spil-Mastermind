namespace Opgave_3_Mastermind.Domain
{
    /// <summary>
    /// Angiver de understøttede sprog i spillet.
    /// </summary>
    /// <remarks>
    /// Denne enum bruges til at angive, hvilket sprog der er valgt af spilleren.
    /// </remarks>
    public enum Sprog
    {
        Da,
        En
    }
    /// <summary>
    /// Udvidelsesmetoder for Sprog-enumen.
    /// </summary>
    /// <remarks>
    /// Denne klasse indeholder metoder til at konvertere mellem strenge og Sprog-enumen.
    /// </remarks>
    public static class SprogExtensions
    {
        public static Sprog ToSprog(this string str) =>
            str.ToLower() switch
            {
                "da" => Sprog.Da,
                "en" => Sprog.En,
                _ => throw new ArgumentException("Ugyldigt sprog"),
            };
    }
}