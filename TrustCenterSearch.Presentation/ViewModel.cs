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

        private string search = string.Empty;
        private string name = string.Empty;
        private string url = string.Empty;
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

        public string Name
        {
            get => this.name;
            set => base.Set(ref this.name, value);
        } 
        public string Url
        {
            get => this.url;
            set => base.Set(ref this.url, value);
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

            if (!Core.ConfigManager.ConfigIsEmpty(Core.Config))
            {
                this.ExecuteSearch();

                foreach (var trustCenter in Core.Config.TrustCenters)
                    TrustCenterHistory.Add(new TrustCenterHistoryElement(trustCenter.Name));
            }
                
        }

        #region CommandHandlings

        private void AddTrustCenterCommand()
        {
            

            if (this.Name == string.Empty)
            {
                MessageBox.Show("There is no name for the TrustCenter", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(this.Name);
                return;
            }

            if (!Core.DownloadManager.IsUrlExisting(this.Url))
            {
                MessageBox.Show("There is no valid url for the TrustCenter", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            Core.Config = Core.ConfigManager.AddTrustCenterToConfig(this.Name, this.Url, Core.Config);
            Core.DownloadManager.DownloadDataFromConfig(Core.Config, Core.DataFolderPath);
            Core.Certificates = Core.DataManager.GetCertificatesFromAppData(Core.Config, Core.DataFolderPath);

            TrustCenterHistory.Add(new TrustCenterHistoryElement(this.name));

            this.ExecuteSearch();
        }
        #endregion

        public void ExecuteSearch()
        {
            if (Core.ConfigManager.ConfigIsEmpty(Core.Config))
                MessageBox.Show("There are no TrustCenters added in the Config", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            this.CertificateSearchResultList.Clear();
            foreach (var certificate in this.Core.SearchManager.GetSearchResults(this.Search, Core.Certificates))
            {
                this.CertificateSearchResultList.Add(certificate);
            }
        }
    }
}
