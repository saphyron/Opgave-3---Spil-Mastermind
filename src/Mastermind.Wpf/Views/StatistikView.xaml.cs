using System.Globalization;
using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Persistence;
using Mastermind.Core.Utils;
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
    public partial class StatistikView : UserControl
    {
        private readonly IStatistikStore _stats;
        private readonly OptionsRepository _optRepo;

        public string Table { get; private set; } = "";

        public RelayCommand ResetCmd   { get; }
        public RelayCommand RefreshCmd { get; }

        // VIGTIGT: ctor matcher det MainViewModel kalder (to argumenter)
        public StatistikView(IStatistikStore stats, OptionsRepository optRepo)
        {
            InitializeComponent();   // ← virker når x:Class + partial + base-klasse matcher
            DataContext = this;

            _stats   = stats;
            _optRepo = optRepo;

            ResetCmd   = new RelayCommand(() => { _stats.Reset(); Refresh(); });
            RefreshCmd = new RelayCommand(Refresh);

            Refresh();
        }

        // i StatisticsView/StatistikView.xaml.cs
private void Refresh()
{
    try
    {
        var list = _stats.LoadAll();
        var agg  = StatistikTilføjer.From(list);

        var opt  = _optRepo.LoadOrDefault();
        var ci   = opt.sprog == Sprog.Da ? CultureInfo.GetCultureInfo("da-DK")
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
