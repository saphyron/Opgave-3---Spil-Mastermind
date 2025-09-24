using Mastermind.Core.Domain;

namespace Mastermind.Konsol.UI
{
    /// <summary>
    /// Håndterer visning af menuer og beskeder i konsollen for Mastermind-spillet.
    /// </summary>
    /// <remarks>
    /// Denne klasse bruger Options til at tilpasse visningen baseret på spillets indstillinger,
    /// såsom sprog og om emojis skal vises.
    /// </remarks>
    public class KonsolMenu
    {
        private readonly Options _options;

        public KonsolMenu(Options options)
        {
            _options = options;
        }
        /// <summary>
        /// Viser hovedmenuen med spillets regler og tilladte farver.
        /// </summary>
        /// <remarks>
        /// Menuen tilpasses baseret på de angivne Options, herunder sprog og om emojis skal vises.
        /// </remarks>
        public void VisMenu()
        {
            Console.WriteLine("MASTER MIND");
            Console.WriteLine($"{Loc("Allowed")}: {string.Join(", ", FarverHelper.AllCanonicalNames(_options.sprog))}");
            Console.WriteLine($"{Loc("HowTo")} {_options.længde} {Loc("Slots")} {Loc("Attempts")}: {_options.maxForsøg}");
            Console.WriteLine();
            if (_options.showEmojis)
            {
                Console.WriteLine($"({Loc("Emojis")})");
            }
            Console.WriteLine();
        }
        /// <summary>
        /// Viser prompten for brugerens gæt, inklusive nuværende forsøg og maksimalt antal forsøg.
        /// </summary>
        /// <param name="forsøg"></param>
        /// <remarks>
        /// Prompten tilpasses baseret på de angivne Options.
        /// </remarks>
        public void VisGætPrompt(int forsøg)
        {
            Console.Write($"{Loc("Guess")} #{forsøg}/{_options.maxForsøg}: ");
        }
        /// <summary>
        /// Viser fejlbeskeder til brugeren.
        /// </summary>
        /// <param name="besked"></param>
        /// <remarks>
        /// Fejlbeskeder vises med en præfiks baseret på det valgte sprog.
        /// </remarks>
        public void VisFejlbesked(string besked)
        {
            Console.WriteLine($"[{Loc("Error")}] {besked}");
            Console.WriteLine();
        }
        /// <summary>
        /// Viser feedback til brugeren baseret på deres gæt.
        /// </summary>
        /// <param name="feedback"></param>
        /// <remarks>
        /// Feedback vises ved hjælp af Respons-klassen, som håndterer formatering baseret på Options.
        /// </remarks>
        public void VisFeedback(Feedback feedback)
        {
            Respons.SkrivRespons(feedback, _options);
            Console.WriteLine();
        }
        /// <summary>
        /// Viser en meddelelse til brugeren, når de har vundet spillet.
        /// </summary>
        /// <param name="forsøg"></param>
        /// <remarks>
        /// Meddelelsen tilpasses baseret på de angivne Options i henhold til sproget. Den inkluderer antallet af forsøg, der blev brugt til at gætte koden korrekt.
        /// </remarks>
        public void VisVindermeddelelse(int forsøg)
        {
            Console.WriteLine($"{Loc("Win")} {forsøg} {Loc("Tries")}.");
        }
        /// <summary>
        /// Viser en meddelelse til brugeren, når de har tabt spillet, inklusive den korrekte hemmelige kode.
        /// </summary>
        /// <param name="secret"></param>
        /// <remarks>
        /// Meddelelsen tilpasses baseret på de angivne Options i henhold til sproget. Den korrekte hemmelige kode vises som en række farvenavne.
        /// </remarks>
        public void VisTabermeddelelse(Farve[] secret)
        {
            var code = string.Join(" ", secret.Select(f => f.ToString()));
            Console.WriteLine($"{Loc("Lose")} {code}");
        }
        /// <summary>
        /// Viser prompten for at spørge brugeren, om de vil spille igen.
        /// </summary>
        /// <remarks>
        /// Prompten tilpasses baseret på de angivne Options i henhold til sproget.
        /// </remarks>
        public void SpilIgen()
        {
            Console.Write($"{Loc("PlayAgain")}?");
        }
        /// <summary>
        /// Viser prompten for at spørge brugeren om et ja/nej-svar.
        /// </summary>
        /// <returns>En streng, der repræsenterer brugerens svar.</returns>
        /// <remarks>
        /// Meddelelsen tilpasses baseret på de angivne Options i henhold til sproget.
        /// </remarks>
        public string YesNo() => Loc("YesNo");
        /// <summary>
        /// Viser en farvelbesked til brugeren, når de afslutter spillet.
        /// </summary>
        /// <remarks>
        /// Beskeden tilpasses baseret på de angivne Options i henhold til sproget.
        /// </remarks>
        public void Farvel() => Console.WriteLine(Loc("Goodbye"));
        /// <summary>
        /// Viser en separator mellem spilrunder for bedre læsbarhed.
        /// </summary>
        /// <remarks>
        /// Separatoren er en simpel linje af bindestreger. Den hjælper med at adskille forskellige sektioner af spillet i konsollen.
        /// </remarks>
        public void RundeSeparator() =>
            Console.WriteLine
                (
                    "----------------------------"
                );
        /// <summary>
        /// Lokaliserer en streng baseret på det valgte sprog i Options.
        /// </summary>
        /// <param name="key"></param>
        /// <remarks>
        /// Denne metode bruger en simpel switch-udtryk til at returnere den korrekte streng baseret på sproget.
        /// Hvis sproget ikke er genkendt, returneres nøglen som standard.
        /// </remarks>
        private string Loc(string key) =>
            _options.sprog == Sprog.Da ? Da(key) : En(key);
        /// <summary>
        /// Danske lokaliserede strenge.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Den lokaliserede danske streng.</returns>
        /// <remarks>
        /// Denne metode bruger et switch-udtryk til at returnere den korrekte danske streng baseret på nøglen.
        /// Hvis nøglen ikke er genkendt, returneres nøglen som standard.
        /// </remarks>
        private static string Da(string key) => key switch
        {
            "Allowed" => "Gyldige farver",
            "HowTo" => "Gæt koden ved at skrive farver adskilt af mellemrum.",
            "Slots" => "pladser",
            "Attempts" => "Forsøg",
            "Guess" => "Gæt",
            "Error" => "Fejl",
            "Win" => "Tillykke! Du gættede koden på",
            "Tries" => "forsøg",
            "Lose" => "Du tabte. Den hemmelige kode var:",
            "Emojis" => "Feedback vil blive vist med emojis: ⚫ for sort og ⚪ for hvid.",
            "PlayAgain" => "Vil du spille igen",
            "YesNo" => "j/n",
            "Goodbye" => "Farvel!",
            _ => key
        };
        /// <summary>
        /// Engelske lokaliserede strenge.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Den lokaliserede engelske streng.</returns>
        /// <remarks>
        /// Denne metode bruger et switch-udtryk til at returnere den korrekte engelske streng baseret på nøglen.
        /// Hvis nøglen ikke er genkendt, returneres nøglen som standard.
        /// </remarks>
        private static string En(string key) => key switch
        {
            "Allowed" => "Allowed colors",
            "HowTo" => "Guess the code by typing colors separated by spaces.",
            "Slots" => "slots",
            "Attempts" => "Attempts",
            "Guess" => "Guess",
            "Error" => "Error",
            "Win" => "Congratulations! You cracked the code in",
            "Tries" => "attempts",
            "Lose" => "You lost. The secret code was:",
            "Emojis" => "Feedback will be shown with emojis: ⚫ for black and ⚪ for white.",
            "PlayAgain" => "Do you want to play again",
            "YesNo" => "y/n",
            "Goodbye" => "Goodbye!",
            _ => key
        };
    }
}