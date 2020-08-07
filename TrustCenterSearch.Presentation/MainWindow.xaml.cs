using System;
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
        
        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            /*sender as TextBox
                this.DataContext as ViewModel*/

        }
    }
}