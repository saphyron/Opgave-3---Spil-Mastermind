using System.Globalization;
using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Persistence;
using Mastermind.Core.Utils;
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
    /// <summary>
    /// Viser en tabel med vedvarende spilstatistik og giver knapper til at nulstille og opdatere.
    /// </summary>
    /// <remarks>
    /// WPF <see cref="UserControl"/> der henter data fra <see cref="IStatistikStore"/>,
    /// aggregerer med <see cref="StatistikTilføjer"/> og formatterer output via kultur
    /// bestemt af <see cref="OptionsRepository"/> (da-DK / en-US). UI bindes til <c>Table</c>,
    /// og kommandoer (<see cref="ResetCmd"/>, <see cref="RefreshCmd"/>) løfter ændringer gennem
    /// <c>DataContext</c>-refresh.
    /// </remarks>
    public partial class StatistikView : UserControl
    {
        private readonly IStatistikStore _stats;
        private readonly OptionsRepository _optRepo;

        public string Table { get; private set; } = "";

        public RelayCommand ResetCmd { get; }
        public RelayCommand RefreshCmd { get; }

        /// <summary>
        /// Initialiserer statistikvisningen, opsætter kommandoer og henter første visning.
        /// </summary>
        /// <param name="stats">Vedvarende lager til statistik.</param>
        /// <param name="optRepo">Repository til indstillinger.</param>
        /// <remarks>
        /// Kommandoer til at nulstille og opdatere statistik oprettes.
        /// Første visning hentes ved start.
        /// </remarks>
        public StatistikView(IStatistikStore stats, OptionsRepository optRepo)
        {
            InitializeComponent();
            DataContext = this;

            _stats = stats;
            _optRepo = optRepo;

            ResetCmd = new RelayCommand(() => { _stats.Reset(); Refresh(); });
            RefreshCmd = new RelayCommand(Refresh);

            Refresh();
        }

        // i StatisticsView/StatistikView.xaml.cs
        /// <summary>
        /// Indlæser statistik, beregner aggregation og opdaterer tabelvisningen.
        /// </summary>
        /// <remarks>
        /// Henter alle resultater fra <see cref="_stats"/>, aggregerer med <see cref="StatistikTilføjer"/>
        /// og formatterer som tabel baseret på sprogindstilling i <see cref="_optRepo"/>.
        /// Opdaterer <c>DataContext</c> for at løfte ændringer til UI.
        /// </remarks>
        private void Refresh()
        {
            try
            {
                var list = _stats.LoadAll();
                var agg = StatistikTilføjer.From(list);

                var opt = _optRepo.LoadOrDefault();
                var ci = opt.sprog == Sprog.Da ? CultureInfo.GetCultureInfo("da-DK")
                                                 : CultureInfo.GetCultureInfo("en-US");

                Table = agg.RenderTable(opt.sprog == Sprog.Da ? "STATISTIK (PERSISTENT)" : "STATISTICS (PERSISTENT)", ci);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Statistics error");
                Table = ex.ToString();
            }

            DataContext = null; DataContext = this;
        }

    }
}
