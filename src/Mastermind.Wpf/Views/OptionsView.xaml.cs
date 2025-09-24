using System.Windows.Controls;
using Mastermind.Core.Domain;
using Mastermind.Core.Persistence;
using Mastermind.Wpf.Infrastructure;

namespace Mastermind.Wpf.Views
{
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

        public OptionsView(OptionsRepository repo)
        {
            InitializeComponent();
            DataContext = this;

            _repo = repo;
            Load();

            SaveCmd = new RelayCommand(Save);
            ReloadCmd = new RelayCommand(Load);
        }

        private void Load()
        {
            var o = _repo.LoadOrDefault();
            Længde = o.længde;
            MaxForsøg = o.maxForsøg;
            ShowEmojis = o.showEmojis;
            SprogValg = o.sprog;

            DataContext = null; DataContext = this;
        }

        private void Save()
        {
            var o = new Options(Længde, MaxForsøg, ShowEmojis, SprogValg);
            _repo.Save(o);
        }
    }
}
