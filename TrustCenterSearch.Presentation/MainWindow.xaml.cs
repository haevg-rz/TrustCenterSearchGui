using System.Windows;
using System.Windows.Controls;
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

        private void CollapseSidebar(object sender, RoutedEventArgs e)
        {
            sidebar.Visibility = System.Windows.Visibility.Collapsed;
            ShowSidebarButton.Visibility = Visibility.Visible;
        }

        private void ShowSidebar(object sender, RoutedEventArgs e)
        {
            sidebar.Visibility = System.Windows.Visibility.Visible;
            ShowSidebarButton.Visibility = Visibility.Collapsed;
        }
    }
}
