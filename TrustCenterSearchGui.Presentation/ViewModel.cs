using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Presentation
{
    public class ViewModel : ViewModelBase
    {
        #region fields

        private string search = "";
        private ObservableCollection<TrustCenterHistoryElement> trustCenterHistory = new ObservableCollection<TrustCenterHistoryElement>();

        #endregion

        #region Properties

        public TrustCenterSearch.Core.Core Core { get; set; }

        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList { get; set; }

        public ObservableCollection<TrustCenterHistoryElement> TrustCenterHistory
        {
            get => this.trustCenterHistory;
            set => base.Set(ref this.trustCenterHistory, value);
        }

        public string Search
        {
            get => this.search;
            set => base.Set(ref this.search, value);
        }

        #endregion

        #region Commands
        public RelayCommand AddTrustCenterButton { get; set; }

        #endregion

        public ViewModel()
        {
            SimpleIoc.Default.Register<ViewModel>();

            this.Core = new TrustCenterSearch.Core.Core();

            CertificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();

            this.AddTrustCenterButton = new RelayCommand(AddTrustCenterCommand);

            ExecuteSearch();
        }

        #region CommandHandlings

        private void AddTrustCenterCommand()
        {
            TrustCenterHistory.Add(new TrustCenterHistoryElement("test"));
            TrustCenterHistory.Add(new TrustCenterHistoryElement("sehrsehrgroßertest"));
        }
        #endregion

        public void ExecuteSearch()
        {
            if (Core.ConfigManager.ConfigIsEmpty(Core.Config))
                MessageBox.Show("There are no TrustCenters added in the Config",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            this.CertificateSearchResultList.Clear();
            foreach (var certificate in this.Core.SearchManager.GetSearchResults(this.Search, Core.Certificates))
            {
                this.CertificateSearchResultList.Add(certificate);
            }
        }
    }
}
