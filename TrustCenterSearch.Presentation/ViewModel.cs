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

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryActive =
            new ObservableCollection<TrustCenterMetaInfo>();

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryInactive =
            new ObservableCollection<TrustCenterMetaInfo>();

        private ICollectionView _certificatesCollectionView;

        #endregion

        #region Properties

        public bool UserInputIsEnabled
        {
            get => _userImputIsEnablet;
            set => Set(ref _userImputIsEnablet, value);
        }

        public Core.Core Core { get; set; }

        public ICollectionView CertificatesCollectionView
        {
            get => _certificatesCollectionView;
            set => Set(ref _certificatesCollectionView, value);
        }

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryActive
        {
            get => _trustCenterHistoryActive;
            set => Set(ref _trustCenterHistoryActive, value);
        }

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryInactive
        {
            get => _trustCenterHistoryInactive;
            set => Set(ref _trustCenterHistoryInactive, value);
        }

        public string SearchBarInput
        {
            get => _searchBarInput;
            set
            {
                Set(ref _searchBarInput, value);
                CertificatesCollectionView.Refresh();
            }
        }

        public string AddTrustCenterName
        {
            get => _addTrustCenterName;
            set => Set(ref _addTrustCenterName, value);
        }

        public string AddTrustCenterUrl
        {
            get => _addTrustCenterUrl;
            set => Set(ref _addTrustCenterUrl, value);
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
            AddTrustCenterButtonCommand = new RelayCommand(AddTrustCenterCommandExecute);
            LoadDataCommand = new RelayCommand(LoadDataCommandExecute);
            AddTrustCenterToFilterCommand = new RelayCommand<TrustCenterMetaInfo>(AddTrustCenterToFilterCommandExecute);
            RemoveTrustCenterFromFilterCommand =
                new RelayCommand<TrustCenterMetaInfo>(RemoveTrustCenterFromFilterCommandExecute);
            DeleteTrustCenterFromHistoryCommand =
                new RelayCommand<TrustCenterMetaInfo>(DeleteTrustCenterFromHistoryCommandExecute);
            InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(InfoAboutTrustCenterCommandExecute);
            RefreshTrustCenterCertificates = new RelayCommand<TrustCenterMetaInfo>(DownloadTrustCenterCertificatesExecute);

            Core = new Core.Core();
        }

        private async void DownloadTrustCenterCertificatesExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.UserInputIsEnabled = false;

            var newTrustCenterMetaInfo = await Core.RefreshTrustCenterCertificates(trustCenterMetaInfo);
            this.RefreshTrustCenterHistoryElement(trustCenterMetaInfo, newTrustCenterMetaInfo);
            
            this.UserInputIsEnabled = true;
        }

        private void RefreshTrustCenterHistoryElement(TrustCenterMetaInfo trustCenterMetaInfoShouldDelete, TrustCenterMetaInfo newTrustCenterMetaInfo)
        {
            if (TrustCenterHistoryActive.Any(tc => tc.Name.Equals(trustCenterMetaInfoShouldDelete.Name)))
            {
                this.TrustCenterHistoryActive.Remove(trustCenterMetaInfoShouldDelete);
            this.TrustCenterHistoryActive.Add(newTrustCenterMetaInfo);
                CertificatesCollectionView.Refresh();
                return;
            }

            this.TrustCenterHistoryInactive.Remove(trustCenterMetaInfoShouldDelete);
            this.TrustCenterHistoryInactive.Add(newTrustCenterMetaInfo);
            
            CertificatesCollectionView.Refresh();
        }

        private async Task Initialize()
        {
            UserInputIsEnabled = false;

            await Core.ImportAllCertificatesFromTrustCenters();

            GetTrustCenterHistory();

            var defaultView = CollectionViewSource.GetDefaultView(Core.GetCertificates());
            defaultView.Filter = Filter;
            CertificatesCollectionView = defaultView;

            UserInputIsEnabled = true;
        }

        private async void LoadDataCommandExecute()
        {
            await Initialize();
        }

        #endregion

        #region Core accessing methods

        private async void AddTrustCenterCommandExecute()
        {
            UserInputIsEnabled = false;
            TrustCenterMetaInfo newTrustCenterMetaInfo;
            try
            {
                newTrustCenterMetaInfo = await Core.AddTrustCenter(AddTrustCenterName, AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                UserInputIsEnabled = true;
                return;
            }

            TrustCenterHistoryActive.Add(newTrustCenterMetaInfo);
            AddTrustCenterName = string.Empty;
            AddTrustCenterUrl = string.Empty;
            CertificatesCollectionView.Refresh();
            UserInputIsEnabled = true;
        }

        private void DeleteTrustCenterFromHistoryCommandExecute(TrustCenterMetaInfo trustCenterToDelete)
        {
            UserInputIsEnabled = false;
            Core.DeleteTrustCenter(trustCenterToDelete);

            TrustCenterHistoryActive.Remove(trustCenterToDelete);
            TrustCenterHistoryInactive.Remove(trustCenterToDelete);

            CertificatesCollectionView.Refresh();
            UserInputIsEnabled = true;
        }

        private void GetTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.GetTrustCenterHistory())
                TrustCenterHistoryActive.Add(trustCenterHistoryName);
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
            TrustCenterHistoryInactive.Remove(trustCenterMetaInfo);
            TrustCenterHistoryActive.Add(trustCenterMetaInfo);
            CertificatesCollectionView.Refresh();
        }

        private void RemoveTrustCenterFromFilterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            TrustCenterHistoryActive.Remove(trustCenterMetaInfo);
            TrustCenterHistoryInactive.Add(trustCenterMetaInfo);
            CertificatesCollectionView.Refresh();
        }

        private bool Filter(object obj)
        {
            if (!(obj is Certificate entry))
                return false;

            if (!TrustCenterHistoryActive.Any(x => x.Name.Equals(entry.TrustCenterName)))
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