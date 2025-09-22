using Opgave_3_Mastermind.Domain;
namespace Opgave_3_Mastermind.Services
{
    public class Input
    {
        private readonly Options _options;
        public Input(Options options)
        {
            _options = options;
        }

        public bool prøvParseGæt(string? linje, out Farve[] gæt, out string? fejl)
        {
            gæt = Array.Empty<Farve>();
            fejl = null;

            if (string.IsNullOrWhiteSpace(linje))
            {
                fejl = _options.sprog == Sprog.En ? "Input cannot be empty." : "Input må ikke være tomt.";
                return false;
            }

            string[] tokens = linje.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != _options.længde)
            {
                fejl = _options.sprog == Sprog.En
                    ? $"Input must contain exactly {_options.længde} colors."
                    : $"Input skal indeholde præcis {_options.længde} farver.";
                return false;
            }

            List<Farve> farver = new List<Farve>();
            foreach (string token in tokens)
            {
                if (Enum.TryParse<Farve>(token, true, out Farve farve))
                {
                    farver.Add(farve);
                }
                else
                {
                    fejl = _options.sprog == Sprog.En
                        ? $"Invalid color: '{token}'. Valid colors are: {string.Join(", ", Enum.GetNames<Farve>())}."
                        : $"Ugyldig farve: '{token}'. Gyldige farver er: {string.Join(", ", Enum.GetNames<Farve>())}.";
                    return false;
                }
            }

            gæt = farver.ToArray();
            return true;
        }
    }
}