using System.Windows;

namespace Mastermind.Wpf
{
    /// <summary>
    /// App’ens hovedvindue der viser indholdet fra <c>MainViewModel</c>.
    /// </summary>
    /// <remarks>
    /// Er en WPF <see cref="Window"/>. Kalder <c>InitializeComponent()</c> (kræver matchende XAML)
    /// og sætter <c>DataContext</c> til en ny <see cref="MainViewModel"/>.
    /// </remarks>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialiserer hovedvinduet og binder til <see cref="MainViewModel"/>.
        /// </summary>
        /// <remarks>
        /// Kalder <c>InitializeComponent()</c> (kræver matchende XAML)
        /// og sætter <c>DataContext</c> til en ny <see cref="MainViewModel"/>.
        /// </remarks>
        public MainWindow()
        {
            InitializeComponent();   // ← denne dukker op, når XAML og .cs matcher
            DataContext = new MainViewModel();
        }
    }
}
