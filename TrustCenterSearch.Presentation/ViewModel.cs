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
        private ObservableCollection<string> _trustCenterHistoryActive = new ObservableCollection<string>();
        private ObservableCollection<string> _trustCenterHistoryInactive = new ObservableCollection<string>();
        private ICollectionView _certificatesCollectionView;

        #endregion

        #region Properties

        public Core.Core Core { get; set; }

        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => base.Set(ref this._certificatesCollectionView, value);
        }

        public ObservableCollection<string> TrustCenterHistoryActive
        {
            get => this._trustCenterHistoryActive;
            set => base.Set(ref this._trustCenterHistoryActive, value);
        }        
        public ObservableCollection<string> TrustCenterHistoryInactive
        {
            get => this._trustCenterHistoryInactive;
            set => base.Set(ref this._trustCenterHistoryInactive, value);
        }

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set
            {
                base.Set(ref this._searchBarInput, value);
                this.CertificatesCollectionView.Refresh();
            }
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
        public RelayCommand AddTrustCenterButtonCommand { get; set; }
        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand<string> AddTrustCenterToFilterCommand { get; set; }
        public RelayCommand<string> RemoveTrustCenterFromFilterCommand { get; set; }
        public RelayCommand<string> DeleteTrustCenterFromHistoryCommand { get; set; }

        #endregion

        public ViewModel()
        {
            this.AddTrustCenterButtonCommand = new RelayCommand(this.AddTrustCenterCommandExecute);
            this.LoadDataCommand = new RelayCommand(this.LoadDataCommandExecute);
            this.AddTrustCenterToFilterCommand = new RelayCommand<string>(this.AddTrustCenterToFilterCommandExecute);
            this.RemoveTrustCenterFromFilterCommand = new RelayCommand<string>(this.RemoveTrustCenterFromFilterCommandExecute);
            this.DeleteTrustCenterFromHistoryCommand = new RelayCommand<string>(this.DeleteTrustCenterFroHistoryCommandExecute);

            this.Core = new Core.Core();
        }

        private async Task Initialize()
        {
            await this.Core.ImportAllCertificatesFromTrustCenters();

            this.LoadTrustCenterHistory();

            var defaultView = CollectionViewSource.GetDefaultView(this.Core.GetCertificates());
            defaultView.Filter = this.Filter;
            this.CertificatesCollectionView = defaultView;
        }

        private async void LoadDataCommandExecute()
        {
            await this.Initialize();
        }

        #region TrustCenterSearchManager Interface

        private void AddTrustCenterToFilterCommandExecute(string trustCenterName)
        {
            TrustCenterHistoryInactive.Remove(trustCenterName);
            TrustCenterHistoryActive.Add(trustCenterName);
        }

        private void RemoveTrustCenterFromFilterCommandExecute(string trustCenterName)
        {
            TrustCenterHistoryActive.Remove(trustCenterName);
            TrustCenterHistoryInactive.Add(trustCenterName);
        }

        private async void AddTrustCenterCommandExecute()
        {
            try
            {
                await Core.AddTrustCenter(this.AddTrustCenterName, this.AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            this.TrustCenterHistoryActive.Add(this._addTrustCenterName);
            CertificatesCollectionView.Refresh();
        }

        private void DeleteTrustCenterFroHistoryCommandExecute(string trustCenterToDelete)
        {
            this.Core.DeleteTrustCenter(trustCenterToDelete);
            this.TrustCenterHistoryActive.Remove(TrustCenterHistoryActive.FirstOrDefault(tch => tch.Equals(trustCenterToDelete)));
            this.CertificatesCollectionView.Refresh();
        }

        private void LoadTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.LoadTrustCenterHistory())
                TrustCenterHistoryActive.Add(trustCenterHistoryName);
        }

        #endregion

        private bool Filter(object obj)
        {
            var entry = obj as Certificate;
            if (entry == null)
                return false;
            if (string.IsNullOrWhiteSpace(this.SearchBarInput))
                return true;

            var searchBarInputToLower = SearchBarInput.ToLower();

            if (entry.Issuer.ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }
            if (entry.Subject.ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }
            if (entry.SerialNumber.ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }
            if (entry.NotBefore.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }
            if (entry.NotAfter.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }
            if (entry.Thumbprint.ToLower().Contains(searchBarInputToLower))
            {
                return true;
            }

            return false;
        }
    }
}
