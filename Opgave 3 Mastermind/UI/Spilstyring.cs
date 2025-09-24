using Opgave_3_Mastermind.Domain;
using Opgave_3_Mastermind.Services;

namespace Opgave_3_Mastermind.UI
{
    public class Spilstyring
    {
        private readonly Options _options;
        private readonly KonsolMenu _menu;
        private readonly SecretGenerator _secretGenerator;
        private readonly Input _input;
        private readonly Evaluering _evaluering;
        /// <summary>
        /// Initialiserer en ny instans af Spilstyring-klassen.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="generator"></param>
        /// <param name="input"></param>
        /// <param name="evaluering"></param>
        /// <param name="menu"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>
        /// Denne klasse håndterer spillets logik og interaktion mellem de forskellige komponenter.
        /// Den bruger Options til at konfigurere spillet, SecretGenerator til at generere hemmelige koder,
        /// Input til at håndtere brugerinput, Evaluering til at evaluere gæt, og KonsolMenu til at vise menuer og beskeder.
        /// </remarks>
        public Spilstyring(
            Options options,
            SecretGenerator generator,
            Input input,
            Evaluering evaluering,
            KonsolMenu menu)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _secretGenerator = generator ?? throw new ArgumentNullException(nameof(generator));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _evaluering = evaluering ?? throw new ArgumentNullException(nameof(evaluering));
            _menu = menu ?? throw new ArgumentNullException(nameof(menu));
        }
        /// <summary>
        /// Starter spillet og håndterer spillets hovedloop.
        /// </summary>
        /// <remarks>
        /// Denne metode viser menuen, håndterer en enkelt runde af spillet, og spørger brugeren om de vil spille igen.
        /// Hvis brugeren vælger ikke at spille igen, afsluttes spillet med en farvelbesked.
        /// </remarks>
        public void Start()
        {
            while (true)
            {
                _menu.VisMenu();
                SingularRunde();

                if (!PrøvIgenJaNej())
                {
                    _menu.Farvel();
                    break;
                }
                _menu.RundeSeparator();
            }
        }
        /// <summary>
        /// Håndterer en enkelt runde af Mastermind-spillet.
        /// </summary>
        /// <remarks>
        /// Denne metode genererer en hemmelig kode, håndterer brugerens gæt, evaluerer gæt, og viser feedback.
        /// Hvis brugeren gætter koden korrekt inden for det maksimale antal forsøg, vinder de spillet.
        /// Hvis de ikke gør, vises den korrekte kode ved spillets afslutning.
        /// </remarks>
        public void SingularRunde()
        {
            var secret = _secretGenerator.GenerateSecret();
            int forsøg = 0;
            bool vundet = false;
            while (forsøg < _options.maxForsøg && !vundet)
            {
                _menu.VisGætPrompt(forsøg + 1);
                var linje = Console.ReadLine();
                if (linje == null)
                {
                    _menu.VisFejlbesked("Input må ikke være null");
                    continue;
                }
                if (!_input.prøvParseGæt(linje, out var gæt, out var fejl))
                {
                    _menu.VisFejlbesked(fejl ?? "Ukendt fejl");
                    continue;
                }
                forsøg++;
                var feedback = _evaluering.Evaluer(gæt, secret);
                _menu.VisFeedback(feedback);
                if (feedback.Black == _options.længde)
                {
                    vundet = true;
                    _menu.VisVindermeddelelse(forsøg);
                    return;
                }
            }
            _menu.VisTabermeddelelse(secret);
        }
        /// <summary>
        /// Spørger brugeren, om de vil spille igen, og returnerer deres svar som en boolesk værdi.
        /// </summary>
        /// <returns>True hvis brugeren vil spille igen, ellers false.</returns>
        /// <remarks>
        /// Denne metode viser en prompt til brugeren og venter på deres svar.
        /// </remarks>
        private bool PrøvIgenJaNej()
        {
            while (true)
            {
                _menu.SpilIgen();
                var svar = Console.ReadLine();
                if (svar == null)
                {
                    _menu.VisFejlbesked("Input må ikke være null");
                    continue;
                }
                svar = svar.Trim().ToLower();
                if (svar == "ja" || svar == "j" || svar == "yes" || svar == "y")
                {
                    return true;
                }
                else if (svar == "nej" || svar == "n" || svar == "no")
                {
                    return false;
                }
                else
                {
                    _menu.VisFejlbesked(_menu.YesNo());
                }
            }
        }
    }
}