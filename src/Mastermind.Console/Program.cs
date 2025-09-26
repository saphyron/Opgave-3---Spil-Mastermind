using Mastermind.Core.Domain;
using Mastermind.Core.Services;
using Mastermind.Konsol.UI;
using Mastermind.Konsol.Utils;
using Mastermind.Core.Utils;
using Mastermind.Core.Persistence;

var repo = new OptionsRepository();
var opts = repo.LoadOrDefault();

var secretGenerator = new SecretGenerator(opts);
var input = new Input(opts);
var evaluering = new Evaluering();
var menu = new KonsolMenu(opts);
var stats = new Statistik();
var statistikStore = new JsonStatistikStore();
// Opret spilstyring med alle afhængigheder
var spilstyring = new Spilstyring(opts, secretGenerator, input, evaluering, menu, stats, statistikStore);
spilstyring.Start();