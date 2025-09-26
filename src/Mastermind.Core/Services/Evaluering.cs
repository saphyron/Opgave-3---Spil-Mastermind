using Mastermind.Core.Domain;
namespace Mastermind.Core.Services
{
    public class Evaluering
    {
        /// <summary>
    /// Evaluerer et gæt mod den hemmelige kode og returnerer antallet af sorte og hvide pegge.
    /// </summary>
    /// <param name="gæt">Array af farver, som repræsenterer spillerens gæt.</param>
    /// <param name="secret">Array af farver, som repræsenterer den hemmelige kode.</param>
    /// <returns>Et <see cref="Feedback"/>-objekt med antal sorte og hvide pegge.</returns>
    /// <remarks>
    /// Metoden sammenligner først hvert element i <paramref name="gæt"/> med det tilsvarende element i <paramref name="secret"/> for at finde sorte pegge (korrekt farve og position).
    /// Derefter gennemgås de resterende elementer for at finde hvide pegge (korrekt farve, forkert position), hvor der tages højde for allerede matchede positioner.
    /// Hvis længden af <paramref name="gæt"/> og <paramref name="secret"/> ikke er ens, kastes en <see cref="ArgumentException"/>.
    /// </remarks>
        public Feedback Evaluer(Farve[] gæt, Farve[] secret)
        {
            if (gæt.Length != secret.Length)
                throw new ArgumentException("Gæt og secret skal have samme længde");

            int black = 0;
            int white = 0;

            bool[] secretBrugt = new bool[secret.Length];
            bool[] gætBrugt = new bool[gæt.Length];

            // Først tjekkes for sorte (black) pegge
            for (int i = 0; i < gæt.Length; i++)
            {
                if (gæt[i] == secret[i])
                {
                    black++;
                    secretBrugt[i] = true;
                    gætBrugt[i] = true;
                }
            }

            // Derefter tjekkes for hvide (white) pegge
            for (int i = 0; i < gæt.Length; i++)
            {
                if (gætBrugt[i]) continue; // Spring over allerede matchede

                for (int j = 0; j < secret.Length; j++)
                {
                    if (secretBrugt[j]) continue; // Spring over allerede matchede

                    if (gæt[i] == secret[j])
                    {
                        white++;
                        secretBrugt[j] = true;
                        break; // Gå til næste gæt
                    }
                }
            }

            return new Feedback(black, white);
        }
    }
}