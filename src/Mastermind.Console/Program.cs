using Mastermind.Core.Domain;
using Mastermind.Core.Services;
using Mastermind.Konsol.UI;
using Mastermind.Konsol.Utils;
using Mastermind.Core.Utils;
using Mastermind.Core.Persistence;
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
var statistikStore = new JsonStatistikStore();
// Opret spilstyring med alle afhængigheder
var spilstyring = new Spilstyring(options, secretGenerator, input, evaluering, menu, stats, statistikStore);
spilstyring.Start();