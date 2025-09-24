using Microsoft.AspNetCore.Mvc.Razor;

namespace Opgave_3_Mastermind.Domain
{
    /// <summary>
    /// Indstillinger for spillet, herunder længde på koden, maksimale forsøg, visning af emojis og sprog.
    /// </summary>
    /// <param name="længde"></param>
    /// <param name="maxForsøg"></param>
    /// <param name="showEmojis"></param>
    /// <param name="sprog"></param>
    /// <remarks>
    /// Denne record bruges til at konfigurere spillets indstillinger og kan nemt udvides med flere indstillinger i fremtiden.
    /// </remarks>
    public record Options(
        int længde = 4,
        int maxForsøg = 12,
        bool showEmojis = true,
        Sprog sprog = Sprog.Da
    );
}