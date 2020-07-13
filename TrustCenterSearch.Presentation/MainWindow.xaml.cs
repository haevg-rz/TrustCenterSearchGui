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

        private void Collapse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
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
