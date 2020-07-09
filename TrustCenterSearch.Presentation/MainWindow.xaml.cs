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

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
