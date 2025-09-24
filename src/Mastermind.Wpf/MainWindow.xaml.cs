using System.Windows;

namespace Mastermind.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();   // ← denne dukker op, når XAML og .cs matcher
            DataContext = new MainViewModel();
        }
    }
}
