using Opgave_3_Mastermind.Domain;
using System;
using Opgave_3_Mastermind.Services;
using System.ComponentModel.Design;
using Microsoft.Extensions.ObjectPool;

namespace Opgave_3_Mastermind.UI
{
    public class Spilstyring
    {
        private readonly Options _options;
        private readonly KonsolMenu _menu;
        private readonly SecretGenerator _secretGenerator;
        private readonly Input _input;
        private readonly Evaluering _evaluering;

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

        //             

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
                _menu.rundeSeperator();
            }
        }

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