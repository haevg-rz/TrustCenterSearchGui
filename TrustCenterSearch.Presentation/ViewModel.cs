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

        private string _searchbarInput = string.Empty;
        private string _addTrustCenterName = string.Empty;
        private string _addTrustCenterUrl = string.Empty;
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

        public string SearchbarInput
        {
            get => this._searchbarInput;
            set => base.Set(ref this._searchbarInput, value);
        }

        public string AddTrustCenterName
        {
            get => this._addTrustCenterName;
            set => base.Set(ref this._addTrustCenterName, value);
        } 
        public string AddTrustCenterUrl
        {
            get => this._addTrustCenterUrl;
            set => base.Set(ref this._addTrustCenterUrl, value);
        }
        #endregion

        #region Commands
        public RelayCommand AddTrustCenterButton { get; set; }

        #endregion

        public ViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            SimpleIoc.Default.Register<ViewModel>();

            this.Core = new Core.Core();

            CertificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();

            this.AddTrustCenterButton = new RelayCommand(AddTrustCenter);

            if (!Core.ConfigManager.IsConfigEmpty(Core.Config))
            {
                this.LoadTrustCenterHistory();
            }
        }

        #region CommandHandlings

        private void AddTrustCenter()
        {
            if (this.AddTrustCenterName == string.Empty)
            {
                MessageBox.Show("There is no _addTrustCenterName for the TrustCenter", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(this.AddTrustCenterName);
                return;
            }

            if (!Core.DownloadManager.IsUrlExisting(this.AddTrustCenterUrl))
            {
                MessageBox.Show("There is no valid _addTrustCenterUrl for the TrustCenter", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Core.AddTrustCenter(this.AddTrustCenterName, this.AddTrustCenterUrl);

            TrustCenterHistory.Add(new TrustCenterHistoryElement(this._addTrustCenterName));

            this.ExecuteSearch();
        }
        #endregion

        public void ExecuteSearch()
        {
            if (Core.ConfigManager.IsConfigEmpty(Core.Config))
                MessageBox.Show("There are no TrustCenters added in the Config", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            this.CertificateSearchResultList.Clear();
            
            foreach (var certificate in this.Core.ExecuteSearch(this.SearchbarInput))
            {
                this.CertificateSearchResultList.Add(certificate);
            }
        }

        private void LoadTrustCenterHistory()
        {
            foreach (var trustCenterHistoryElement in Core.LoadTrustCenterHistory())
                TrustCenterHistory.Add(trustCenterHistoryElement);

            this.ExecuteSearch();
        }
    }
}
