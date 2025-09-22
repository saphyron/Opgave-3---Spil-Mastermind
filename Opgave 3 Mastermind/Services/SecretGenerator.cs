using Opgave_3_Mastermind.Domain;
namespace Opgave_3_Mastermind.Services
{
    public class SecretGenerator
    {
        private readonly Random _random = Random.Shared;
        private readonly Farve[] _farver;
        private readonly Options _options;

        public SecretGenerator(Options options)
        {
            _options = options;
            _farver = Enum.GetValues<Farve>();
        }

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