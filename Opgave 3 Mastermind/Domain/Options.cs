using Microsoft.AspNetCore.Mvc.Razor;

namespace Opgave_3_Mastermind.Domain
{
    public record Options(
        int længde = 4,
        int maxForsøg = 12,
        bool showEmojis = true,
        Sprog sprog = Sprog.Da
    );
}