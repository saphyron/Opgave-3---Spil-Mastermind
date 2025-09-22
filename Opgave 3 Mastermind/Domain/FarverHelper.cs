using System.Globalization;

namespace Opgave_3_Mastermind.Domain
{
    public static class FarverHelper
    {
        private static readonly Dictionary<string, Farve> FarveMap = new Dictionary<string, Farve>(StringComparer.OrdinalIgnoreCase)
        {
            // Danske
            { "RØD", Farve.Rød },
            { "BLÅ", Farve.Blå },
            { "GRØN", Farve.Grøn },
            { "GUL", Farve.Gul },
            { "SORT", Farve.Sort },
            { "HVID", Farve.Hvid },
            {"lilla", Farve.Lilla },
            {"orange", Farve.Orange },
            // Engelske
            { "RED", Farve.Rød },
            { "BLUE", Farve.Blå },
            { "GREEN", Farve.Grøn },
            { "YELLOW", Farve.Gul },
            { "BLACK", Farve.Sort },
            { "WHITE", Farve.Hvid },
            {"purple", Farve.Lilla },
        };

        // -------- Visningsnavne (ét pr. farve pr. sprog) --------
        private static readonly Dictionary<Farve, string> _da = new()
        {
            [Farve.Rød] = "rød",
            [Farve.Blå] = "blå",
            [Farve.Grøn] = "grøn",
            [Farve.Gul] = "gul",
            [Farve.Sort] = "sort",
            [Farve.Hvid] = "hvid",
            [Farve.Lilla] = "lilla",
            [Farve.Orange] = "orange",
        };

        private static readonly Dictionary<Farve, string> _en = new()
        {
            [Farve.Rød] = "red",
            [Farve.Blå] = "blue",
            [Farve.Grøn] = "green",
            [Farve.Gul] = "yellow",
            [Farve.Sort] = "black",
            [Farve.Hvid] = "white",
            [Farve.Lilla] = "purple",
            [Farve.Orange] = "orange",
        };

        private static readonly Farve[] _palette = (Farve[])Enum.GetValues(typeof(Farve));
        public static bool TryParse(string input, out Farve farve)
        {
            if (input == null)
            {
                farve = default;
                return false;
            }
            return FarveMap.TryGetValue(input.Trim(), out farve);
        }

        public static string ToName(Farve farve, Sprog language) =>
            (language == Sprog.Da ? _da : _en).TryGetValue(farve, out var name)
                ? name
                : farve.ToString();
        public static IReadOnlyList<string> AllCanonicalNames(Sprog language) =>
            _palette.Select(c => ToName(c, language)).ToArray();

        /// <summary>Hele paletten (enumrækkefølge).</summary>
        public static IReadOnlyList<Farve> Palette => _palette;
    }
}