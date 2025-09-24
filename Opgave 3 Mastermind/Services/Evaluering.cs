using Opgave_3_Mastermind.Domain;
namespace Opgave_3_Mastermind.Services
{
    /// <summary>
    /// Evaluerer et gæt mod den hemmelige kode og returnerer feedback i form af sorte og hvide pegge.
    /// </summary>
    /// <param name="gæt">Et array af Farve, der repræsenterer spillerens gæt.</param>
    /// <param name="secret">Et array af Farve, der repræsenterer den hemmelige kode.</param>
    /// <returns>En Feedback-instans, der indeholder antallet af sorte og hvide pegge.</returns>
    /// <remarks>
    /// Denne metode evaluerer et gæt ved at sammenligne det med den hemmelige kode
    /// og returnerer feedback i form af sorte og hvide pegge.
    /// </remarks>
    public class Evaluering
    {
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