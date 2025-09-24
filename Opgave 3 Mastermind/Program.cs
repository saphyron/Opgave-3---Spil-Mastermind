using Opgave_3_Mastermind.Domain;
using Opgave_3_Mastermind.Services;
using Opgave_3_Mastermind.UI;
using Opgave_3_Mastermind.Utils;

//var options = new Options(); // Default options
/* Du kan også ændre options her, hvis du vil teste forskellige indstillinger
options.maxForsøg = 9;
options.længde = 3;
options.showEmojis = true;
options.sprog = Sprog.En;
*/
var options = new Options(længde: 3, maxForsøg: 9, showEmojis: true, sprog: Sprog.En);

var secretGenerator = new SecretGenerator(options);
var input = new Input(options);
var evaluering = new Evaluering();
var menu = new KonsolMenu(options);
var stats = new Statistik();
// Opret spilstyring med alle afhængigheder
var spilstyring = new Spilstyring(options, secretGenerator, input, evaluering, menu, stats);
spilstyring.Start();