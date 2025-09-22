using Opgave_3_Mastermind.Domain;
using Opgave_3_Mastermind.Services;
using Opgave_3_Mastermind.UI;

var options = new Options(); // Default options
/* You can customize options here if needed
options.maxForsøg = 10;
options.længde = 5;
options.showEmojis = false;
options.sprog = Sprog.En;

var options = new Options(længde: 5, maxForsøg: 10, showEmojis: false, sprog: Sprog.En);
*/
var secretGenerator = new SecretGenerator(options);
var input = new Input(options);
var evaluering = new Evaluering();
var menu = new KonsolMenu(options);

var spilstyring = new Spilstyring(options, secretGenerator, input, evaluering, menu);
spilstyring.Start();