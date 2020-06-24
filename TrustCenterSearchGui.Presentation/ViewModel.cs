using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {

        public ViewModel()
        {
            this.CertificateSearchResultList = GetMatchingCertificatesFromSearch(this.search);
            this.RefreshButton = new RelayCommand(RefreshAndSearchCommand);
            this.AddTrustCenterButton = new RelayCommand(AddTrustCenterCommand);
        }

        public RelayCommand RefreshButton { get; set; }
        public RelayCommand CollapseButton { get; set; }
        public RelayCommand AddTrustCenterButton { get; set; }

        private string search;
        public string Search 
        { 
            get => this.search;
            set
            {
                base.Set(ref this.search, value);
                this.CertificateSearchResultList = GetMatchingCertificatesFromSearch(this.search);
            }
        }

        private ObservableCollection<SearchResultsAndBorder> certificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();
        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList
        { 
            get => this.certificateSearchResultList; 
            set => base.Set(ref this.certificateSearchResultList, value);
        }

        private ObservableCollection<SearchResultsAndBorder> GetMatchingCertificatesFromSearch(string searchInput)
        {
            return TrustCenterSearchGui.Core.Core.Searcher(searchInput);
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
            this.CertificateSearchResultList = GetMatchingCertificatesFromSearch(this.search);
            if (Core.Core.ConfigIsEmpty())
                MessageBox.Show("There are no TrustCenters added in the Config",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
