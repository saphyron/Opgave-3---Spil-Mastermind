namespace Mastermind.Core.Domain;
/// <summary>
/// Én spilsession (én runde) resultat
/// </summary>
/// <param name="TimestampUtc"></param>
/// <param name="Vundet"></param>
/// <param name="Forsøg"></param>
/// <remarks>
/// Gemmes i statistik-filen som JSON.
/// </remarks>
public sealed record GameResult(
    DateTime TimestampUtc,   // gem UTC i filen
    bool Vundet,
    int Forsøg
);
