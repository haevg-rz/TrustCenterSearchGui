using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Presentation
{
    public class ViewModel : ViewModelBase
    {
        #region fields

        private string _searchBarInput = string.Empty;
        private string _addTrustCenterName = string.Empty;
        private string _addTrustCenterUrl = string.Empty;
        private bool _userImputIsEnablet = true;

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryActive = new ObservableCollection<TrustCenterMetaInfo>();

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryInactive = new ObservableCollection<TrustCenterMetaInfo>();

        private ICollectionView _certificatesCollectionView;

        #endregion

        #region Properties

        public bool UserInputIsEnabled
        {
            get => this._userImputIsEnablet;
            set => Set(ref this._userImputIsEnablet, value);
        }

        public Core.Core Core { get; set; }

        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => Set(ref this._certificatesCollectionView, value);
        }

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryActive
        {
            get => this._trustCenterHistoryActive;
            set => Set(ref this._trustCenterHistoryActive, value);
        }

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryInactive
        {
            get => this._trustCenterHistoryInactive;
            set => Set(ref this._trustCenterHistoryInactive, value);
        }

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set
            {
                Set(ref this._searchBarInput, value);
                CertificatesCollectionView.Refresh();
            }
        }

        public string AddTrustCenterName
        {
            get => this._addTrustCenterName;
            set => Set(ref this._addTrustCenterName, value);
        }

        public string AddTrustCenterUrl
        {
            get => this._addTrustCenterUrl;
            set => Set(ref this._addTrustCenterUrl, value);
        }

        #endregion

        #region Commands

        public RelayCommand AddTrustCenterButtonCommand { get; set; }
        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> AddTrustCenterToFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> RemoveTrustCenterFromFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> DeleteTrustCenterFromHistoryCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> InfoAboutTrustCenterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> RefreshTrustCenterCertificates { get; set; }

        #endregion

        #region Initialization

        public ViewModel()
        {
            AddTrustCenterButtonCommand = new RelayCommand(this.AddTrustCenterCommandExecute);
            LoadDataCommand = new RelayCommand(this.LoadDataCommandExecute);
            AddTrustCenterToFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.AddTrustCenterToFilterCommandExecute);
            RemoveTrustCenterFromFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.RemoveTrustCenterFromFilterCommandExecute);
            DeleteTrustCenterFromHistoryCommand = new RelayCommand<TrustCenterMetaInfo>(this.DeleteTrustCenterFromHistoryCommandExecute);
            InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(InfoAboutTrustCenterCommandExecute);
            RefreshTrustCenterCertificates = new RelayCommand<TrustCenterMetaInfo>(this.DownloadTrustCenterCertificatesExecute);

            Core = new Core.Core();
        }

        private async void DownloadTrustCenterCertificatesExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.UserInputIsEnabled = false;

            var newTrustCenterMetaInfo = await this.Core.RefreshTrustCenterCertificates(trustCenterMetaInfo);
            this.RefreshTrustCenterHistoryElement(trustCenterMetaInfo, newTrustCenterMetaInfo);

            this.UserInputIsEnabled = true;
        }

        private void RefreshTrustCenterHistoryElement(TrustCenterMetaInfo trustCenterMetaInfoShouldDelete,
            TrustCenterMetaInfo newTrustCenterMetaInfo)
        {
            if (TrustCenterHistoryActive.Any(tc => tc.Name.Equals(trustCenterMetaInfoShouldDelete.Name)))
            {
                this.TrustCenterHistoryActive.Remove(trustCenterMetaInfoShouldDelete);
                this.TrustCenterHistoryActive.Add(newTrustCenterMetaInfo);
                this.CertificatesCollectionView.Refresh();
                return;
            }

            this.TrustCenterHistoryInactive.Remove(trustCenterMetaInfoShouldDelete);
            this.TrustCenterHistoryInactive.Add(newTrustCenterMetaInfo);

            this.CertificatesCollectionView.Refresh();
        }

        private async Task Initialize()
        {
            this.UserInputIsEnabled = false;

            await Core.ImportAllCertificatesFromTrustCenters();

            this.GetTrustCenterHistory();

            var defaultView = CollectionViewSource.GetDefaultView(this.Core.GetCertificates());
            defaultView.Filter = Filter;
            this.CertificatesCollectionView = defaultView;

            this.UserInputIsEnabled = true;
        }

        private async void LoadDataCommandExecute()
        {
            await Initialize();
        }

        #endregion

        #region Core accessing methods

        private async void AddTrustCenterCommandExecute()
        {
            this.UserInputIsEnabled = false;
            TrustCenterMetaInfo newTrustCenterMetaInfo;
            try
            {
                newTrustCenterMetaInfo = await this.Core.AddTrustCenter(this.AddTrustCenterName, this.AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                this.UserInputIsEnabled = true;
                return;
            }

            this.TrustCenterHistoryActive.Add(newTrustCenterMetaInfo);
            this.AddTrustCenterName = string.Empty;
            this.AddTrustCenterUrl = string.Empty;
            this.CertificatesCollectionView.Refresh();
            this.UserInputIsEnabled = true;
        }

        private void DeleteTrustCenterFromHistoryCommandExecute(TrustCenterMetaInfo trustCenterToDelete)
        {
            this.UserInputIsEnabled = false;
            this.Core.DeleteTrustCenter(trustCenterToDelete);

            this.TrustCenterHistoryActive.Remove(trustCenterToDelete);
            this.TrustCenterHistoryInactive.Remove(trustCenterToDelete);

            this.CertificatesCollectionView.Refresh();
            this.UserInputIsEnabled = true;
        }

        private void GetTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in this.Core.GetTrustCenterHistory())
                this.TrustCenterHistoryActive.Add(trustCenterHistoryName);
        }

        #endregion

        #region UI-only methods

        private static void InfoAboutTrustCenterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            MessageBox.Show(
                $"Name: {trustCenterMetaInfo.Name}\n{trustCenterMetaInfo.TrustCenterUrl}\nLast Update: {trustCenterMetaInfo.LastUpdate}",
                "Information about TrustCenter", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddTrustCenterToFilterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.TrustCenterHistoryInactive.Remove(trustCenterMetaInfo);
            this.TrustCenterHistoryActive.Add(trustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();
        }

        private void RemoveTrustCenterFromFilterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.TrustCenterHistoryActive.Remove(trustCenterMetaInfo);
            this.TrustCenterHistoryInactive.Add(trustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();
        }

        private bool Filter(object obj)
        {
            if (!(obj is Certificate entry))
                return false;

            if (!this.TrustCenterHistoryActive.Any(x => x.Name.Equals(entry.TrustCenterName)))
                return false;

            if (string.IsNullOrWhiteSpace(SearchBarInput))
                return true;

            var searchBarInputToLower = SearchBarInput.ToLower();

            if (entry.Issuer.ToLower().Contains(searchBarInputToLower)) return true;
            if (entry.Subject.ToLower().Contains(searchBarInputToLower)) return true;
            if (entry.SerialNumber.ToLower().Contains(searchBarInputToLower)) return true;
            if (entry.NotBefore.ToString(CultureInfo.InvariantCulture).ToLower()
                .Contains(searchBarInputToLower)) return true;
            if (entry.NotAfter.ToString(CultureInfo.InvariantCulture).ToLower()
                .Contains(searchBarInputToLower)) return true;
            if (entry.Thumbprint.ToLower().Contains(searchBarInputToLower)) return true;

            return false;
        }

        #endregion
    }
}