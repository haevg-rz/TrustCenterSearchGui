using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrustCenterSearchGui.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string waterMark = "search...";
        public MainWindow()
        {
            InitializeComponent();

            searchBox.Text = this.waterMark;

            searchBox.GotFocus += RemoveText;
            searchBox.LostFocus += AddText;
        }

        public void RemoveText(object sender, EventArgs e)
        {
            if (searchBox.Text == this.waterMark)
            {
                searchBox.Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBox.Text))
                searchBox.Text = this.waterMark;
        }

    }
}
