using Opgave_3_Mastermind.Domain;
namespace Opgave_3_Mastermind.Services
{
    /// <summary>
    /// Genererer en tilfældig hemmelig kode bestående af farver.
    /// </summary>
    /// <param name="options">Indstillinger for spillet, herunder længde på koden.</param>
    /// <param name="random">En tilfældighedsgenerator til at vælge farver.</param>
    /// <param name="farver">En liste over mulige farver at vælge imellem.</param>
    /// <remarks>
    /// Denne klasse genererer en hemmelig kode ved hjælp af de angivne indstillinger og farver.
    /// </remarks>
    public class SecretGenerator
    {
        private readonly Random _random = Random.Shared;
        private readonly Farve[] _farver;
        private readonly Options _options;
        /// <summary>
        /// Initialiserer en ny instans af SecretGenerator-klassen.
        /// </summary>
        /// <param name="options"></param>
        /// <remarks>
        /// Denne klasse bruger Options til at bestemme længden af den hemmelige kode.
        /// </remarks>
        public SecretGenerator(Options options)
        {
            _options = options;
            _farver = Enum.GetValues<Farve>();
        }
        /// <summary>
        /// Genererer en tilfældig hemmelig kode baseret på de angivne indstillinger.
        /// </summary>
        /// <returns>En array af Farve, der repræsenterer den hemmelige kode.</returns>
        /// <remarks>
        /// Denne metode genererer en hemmelig kode ved hjælp af de angivne indstillinger og farver.
        /// </remarks>
        public Farve[] GenerateSecret()
        {
            Farve[] secret = new Farve[_options.længde];
            for (int i = 0; i < _options.længde; i++)
            {
                secret[i] = _farver[_random.Next(_farver.Length)];
            }
            return secret;
        }

    }
}