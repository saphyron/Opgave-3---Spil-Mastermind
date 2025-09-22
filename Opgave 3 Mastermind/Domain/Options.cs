using Microsoft.AspNetCore.Mvc.Razor;

namespace Opgave_3_Mastermind.Domain
{
    public record Options(
        int længde = 3,
        int maxForsøg = 9,
        bool showEmojis = true,
        Sprog sprog = Sprog.Da
    );
}