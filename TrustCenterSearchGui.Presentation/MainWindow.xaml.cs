using System.Windows;
using System.Windows.Input;

namespace TrustCenterSearch.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Collapse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SearchBoxInputDone(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            (this.DataContext as ViewModel).ExecuteSearch();
        }
    }
}
