using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Persistence;
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
    /// <summary>
    /// Brugerflade til at ændre spilindstillinger som længde, max forsøg, emojis og sprog.
    /// </summary>
    /// <remarks>
    /// Implementerer en WPF <see cref="UserControl"/> hvor indstillinger bindes direkte til felter
    /// og kan gemmes eller genindlæses via <see cref="OptionsRepository"/>.
    /// Indeholder kommandoer til Save og Reload uden brug af separat ViewModel.
    /// </remarks>
    public partial class OptionsView : UserControl
    {
        private readonly OptionsRepository _repo;

        // Bindbare felter (enkelt i code-behind for at undgå ekstra VM-klasse)
        public int Længde { get; set; }
        public int MaxForsøg { get; set; }
        public bool ShowEmojis { get; set; }
        public Sprog SprogValg { get; set; }

        public RelayCommand SaveCmd { get; }
        public RelayCommand ReloadCmd { get; }
        /// <summary>
        /// Initialiserer indstillingsvinduet og loader eksisterende konfiguration.
        /// </summary>
        /// <param name="repo">Repository til indstillinger.</param>
        /// <remarks>
        /// Kommandoer til at gemme og genindlæse indstillinger oprettes.
        /// Indstillinger indlæses ved start.
        /// </remarks>
        public OptionsView(OptionsRepository repo)
        {
            InitializeComponent();
            DataContext = this;

            _repo = repo;
            Load();

            SaveCmd = new RelayCommand(Save);
            ReloadCmd = new RelayCommand(Load);
        }
        /// <summary>
        /// Indlæser indstillinger fra repository og opdaterer bindings.
        /// </summary>
        /// <remarks>
        /// Hvis indstillinger ikke findes, indlæses og vises default-værdier
        /// (som også gemmes til fil).
        /// </remarks>
        private void Load()
        {
            var o = _repo.LoadOrDefault();
            Længde = o.længde;
            MaxForsøg = o.maxForsøg;
            ShowEmojis = o.showEmojis;
            SprogValg = o.sprog;

            DataContext = null; DataContext = this;
        }
        /// <summary>
        /// Gemmer nuværende indstillinger og opdaterer sprog i runtime.
        /// </summary>
        /// <remarks>
        /// Indstillinger gemmes til fil, og alle DynamicResource-bindings opdateres
        /// til det valgte sprog.
        /// </remarks>
        private void Save()
        {
            var o = new Options(Længde, MaxForsøg, ShowEmojis, SprogValg);
            _repo.Save(o);

            // Skift sprog i runtime (opdaterer alle DynamicResource-bindings)
            Localization.SetLanguage(SprogValg);
        }

    }
}
