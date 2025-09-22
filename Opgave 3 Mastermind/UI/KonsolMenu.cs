using Opgave_3_Mastermind.Domain;

namespace Opgave_3_Mastermind.UI
{
    public class KonsolMenu
    {
        private readonly Options _options;

        public KonsolMenu(Options options)
        {
            _options = options;
        }
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
        public void VisGætPrompt(int forsøg)
        {
            Console.Write($"{Loc("Guess")} #{forsøg}/{_options.maxForsøg}: ");
        }
        public void VisFejlbesked(string besked)
        {
            Console.WriteLine($"[{Loc("Error")}] {besked}");
            Console.WriteLine();
        }
        public void VisFeedback(Feedback feedback)
        {
            Respons.SkrivRespons(feedback, _options);
            Console.WriteLine();
        }
        public void VisVindermeddelelse(int forsøg)
        {
            Console.WriteLine($"{Loc("Win")} {forsøg} {Loc("Tries")}.");
        }
        public void VisTabermeddelelse(Farve[] secret)
        {
            var code = string.Join(" ", secret.Select(f => f.ToString()));
            Console.WriteLine($"{Loc("Lose")} {code}");
        }
        public void SpilIgen()
        {
            Console.Write($"{Loc("PlayAgain")}?");
        }
        public string YesNo() => Loc("YesNo");

        public void Farvel() => Console.WriteLine(Loc("Goodbye"));

        public void rundeSeperator() => 
            Console.WriteLine
                (
                    "----------------------------"
                );

        private string Loc(string key) =>
            _options.sprog == Sprog.Da ? Da(key) : En(key);
        
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