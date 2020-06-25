using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {

        public ViewModel()
        {
            SimpleIoc.Default.Register<ViewModel>();
            this.CertificateSearchResultList = GetMatchingCertificatesFromSearch(this.search);
            this.RefreshButton = new RelayCommand(RefreshAndSearchCommand);
            this.AddTrustCenterButton = new RelayCommand(AddTrustCenterCommand);
        }

        public RelayCommand RefreshButton { get; set; }
        public RelayCommand CollapseButton { get; set; }
        public RelayCommand AddTrustCenterButton { get; set; }

        private string search = "";
        public string Search 
        { 
            get => this.search;
            set => base.Set(ref this.search, value);
        }

        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList { get; set;}

        private ObservableCollection<SearchResultsAndBorder> GetMatchingCertificatesFromSearch(string searchInput)
        {
            return new ObservableCollection<SearchResultsAndBorder>(TrustCenterSearchGui.Core.Core.Searcher(searchInput));
        }

        private ObservableCollection<TrustCenterHistoryElement> trustCenterHistory = new ObservableCollection<TrustCenterHistoryElement>();
        public ObservableCollection<TrustCenterHistoryElement> TrustCenterHistory
        {
            get => this.trustCenterHistory;
            set => base.Set(ref this.trustCenterHistory, value);
        }

        private void AddTrustCenterCommand()
        {
            TrustCenterHistory.Add(new TrustCenterHistoryElement("test"));
            TrustCenterHistory.Add(new TrustCenterHistoryElement("sehrsehrgroßertest"));
        }

        private void RefreshAndSearchCommand()
        {
            TrustCenterSearchGui.Core.Core.RefreshButtonCommand();
            this.CertificateSearchResultList.Clear();
            this.ExecuteSearch();
            if (Core.Core.ConfigIsEmpty())
                MessageBox.Show("There are no TrustCenters added in the Config",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ExecuteSearch(string searchInput = "")
        {
            this.CertificateSearchResultList.Clear();
            foreach (var certificate in this.GetMatchingCertificatesFromSearch(searchInput))
            {
                this.CertificateSearchResultList.Add(certificate);
            }
        }
    }
}
