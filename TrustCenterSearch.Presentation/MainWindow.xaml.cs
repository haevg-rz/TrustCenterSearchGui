using System;
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

        private void WatermarkVisibilityConverter(object sender, KeyEventArgs e)
        {
            if (this.SearchBar.Text.Equals(String.Empty))
            {
                this.SearchWatermark.Visibility = Visibility.Visible;
                return;
            }

            this.SearchWatermark.Visibility = Visibility.Collapsed;
        }
    }
}