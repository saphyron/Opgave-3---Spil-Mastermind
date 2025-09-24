using Mastermind.Core.Domain;
namespace Mastermind.Core.Services
{
    /// <summary>
    /// Håndterer brugerinput og validering af gæt.
    /// </summary>
    /// <remarks>
    /// Denne klasse bruger Options til at tilpasse valideringen baseret på spillets indstillinger,
    /// såsom længde på koden og sprog.
    /// </remarks>
    public class Input
    {
        private readonly Options _options;
        public Input(Options options)
        {
            _options = options;
        }
        /// <summary>
        /// Prøver at parse brugerens input til et gæt bestående af farver.
        /// </summary>
        /// <param name="linje"></param>
        /// <param name="gæt"></param>
        /// <param name="fejl"></param>
        /// <returns>True hvis parsing lykkedes, ellers false.</returns>
        /// <remarks>
        /// Denne metode forsøger at parse en linje af tekst til et gæt bestående af farver.
        /// </remarks>
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