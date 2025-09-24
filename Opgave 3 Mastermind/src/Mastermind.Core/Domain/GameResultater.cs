namespace Mastermind.Core.Domain;

public sealed record GameResult(
    DateTime TimestampUtc,   // gem UTC i filen
    bool Vundet,
    int Forsøg
);
