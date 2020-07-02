using System;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TrustCenterSearch.Core.Models;
using TrustCenterSearch.Presentation.Models;

namespace TrustCenterSearch.Presentation
{
    public class ViewModel : ViewModelBase
    {
        #region fields

        private string _searchBarInput = string.Empty;
        private string _addTrustCenterName = string.Empty;
        private string _addTrustCenterUrl = string.Empty;
        private ObservableCollection<string> _trustCenterHistory = new ObservableCollection<string>();

        #endregion

        #region Properties

        public Core.Core Core { get; set; }

        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList { get; set; }

        public ObservableCollection<string> TrustCenterHistory
        {
            get => this._trustCenterHistory;
            set => base.Set(ref this._trustCenterHistory, value);
        }

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set => base.Set(ref this._searchBarInput, value);
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

            this.LoadTrustCenterHistory();
        }

        #region TrustCenterSearchManager Interface

        private void AddTrustCenter()
        {
            try
            {
                Core.AddTrustCenter(this.AddTrustCenterName, this.AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TrustCenterHistory.Add(this._addTrustCenterName);
        }

        public void ExecuteSearch()
        {
            this.CertificateSearchResultList.Clear();
            try
            {
                foreach (var certificate in this.Core.ExecuteSearch(this.SearchBarInput))
                {
                    this.CertificateSearchResultList.Add(certificate);
                }
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.LoadTrustCenterHistory())
                TrustCenterHistory.Add(trustCenterHistoryName);

            if(this.TrustCenterHistory.Count>0)
                this.ExecuteSearch();
        }

        #endregion
    }
}
