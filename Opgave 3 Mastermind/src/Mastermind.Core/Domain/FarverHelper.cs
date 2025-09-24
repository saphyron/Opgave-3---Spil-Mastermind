namespace Mastermind.Core.Domain
{
    /// <summary>
    /// Hjælpeklasse til håndtering af farver i Mastermind-spillet.
    /// </summary>
    /// <remarks>
    /// Denne klasse indeholder metoder til at parse farver fra strenge, konvertere farver til visningsnavne og hente hele farvepaletten.
    /// </remarks>
    public static class FarverHelper
    {
        /// <summary>
        /// Mapning af strenge til Farve-enumværdier (både danske og engelske navne).
        /// </summary>
        /// <remarks>
        /// Denne mappe bruges til at konvertere brugerinput til de korrekte Farve-enumværdier.
        /// </remarks>
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

        /// <summary>
        /// Mapning af Farve-enumværdier til deres kanoniske navne på dansk og engelsk.
        /// </summary>
        /// <remarks>
        /// Disse 2 funktioner bruges til at konvertere Farve-enumværdier til visningsvenlige navne baseret på det valgte sprog.
        /// </remarks>
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
        /// <summary>
        /// Hele farvepaletten i enum-rækkefølge.
        /// </summary>
        /// <remarks>
        /// Denne liste indeholder alle Farve-enumværdier i den rækkefølge, de er defineret i enumen.
        /// </remarks>
        private static readonly Farve[] _palette = (Farve[])Enum.GetValues(typeof(Farve));
        /// <summary>
        /// Prøver at parse en streng til en Farve-enumværdi.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="farve"></param>
        /// <returns>True hvis parsing lykkedes, ellers false.</returns>
        /// <remarks>
        /// Denne metode forsøger at konvertere en given streng til en Farve-enumværdi.
        /// </remarks>
        public static bool TryParse(string input, out Farve farve)
        {
            if (input == null)
            {
                farve = default;
                return false;
            }
            return FarveMap.TryGetValue(input.Trim(), out farve);
        }
        /// <summary>
        /// Konverterer en Farve-enumværdi til dens kanoniske navn baseret på det valgte sprog.
        /// </summary>
        /// <param name="farve"></param>
        /// <param name="language"></param>
        /// <returns>Den kanoniske strengrepræsentation af farven.</returns>
        /// <remarks>
        /// Denne metode returnerer det kanoniske navn for en given farve baseret på det valgte sprog.
        /// </remarks>
        public static string ToName(Farve farve, Sprog language) =>
            (language == Sprog.Da ? _da : _en).TryGetValue(farve, out var name)
                ? name
                : farve.ToString();
        /// <summary>
        /// Henter alle kanoniske navne for farverne i paletten baseret på det valgte sprog.
        /// </summary>
        /// <param name="language"></param>
        /// <returns>En liste af strenge, der repræsenterer de kanoniske navne for farverne.</returns>
        /// <remarks>
        /// Denne metode returnerer en liste over alle kanoniske navne for farverne i paletten baseret på det valgte sprog.
        /// </remarks>
        public static IReadOnlyList<string> AllCanonicalNames(Sprog language) =>
            _palette.Select(c => ToName(c, language)).ToArray();

        /// <summary>
        /// Henter hele farvepaletten i enum-rækkefølge.
        /// </summary>
        /// <remarks>
        /// Denne egenskab returnerer en liste over alle Farve-enumværdier i den rækkefølge, de er defineret i enumen.
        /// </remarks>
        public static IReadOnlyList<Farve> Palette => _palette;
    }
}