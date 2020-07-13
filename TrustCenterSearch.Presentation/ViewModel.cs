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
        public void SidebarCommandExecute()
        {
            MessageBox.Show("test");
            
        }

        #region fields

        private string _searchBarInput = string.Empty;
        private string _addTrustCenterName = string.Empty;
        private string _addTrustCenterUrl = string.Empty;
        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryActive = new ObservableCollection<TrustCenterMetaInfo>();
        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryInactive = new ObservableCollection<TrustCenterMetaInfo>();
        private ICollectionView _certificatesCollectionView;

        #endregion

        #region Properties

        public Core.Core Core { get; set; }

        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => base.Set(ref this._certificatesCollectionView, value);
        }

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryActive
        {
            get => this._trustCenterHistoryActive;
            set => base.Set(ref this._trustCenterHistoryActive, value);
        }        
        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryInactive
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
        public RelayCommand SidebarCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> AddTrustCenterToFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> RemoveTrustCenterFromFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> DeleteTrustCenterFromHistoryCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> InfoAboutTrustCenterCommand { get; set; }
        

        #endregion

        public ViewModel()
        {
            this.AddTrustCenterButtonCommand = new RelayCommand(this.AddTrustCenterCommandExecute);
            this.LoadDataCommand = new RelayCommand(this.LoadDataCommandExecute);
            this.SidebarCommand = new RelayCommand(this.SidebarCommandExecute);
            this.AddTrustCenterToFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.AddTrustCenterToFilterCommandExecute);
            this.RemoveTrustCenterFromFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.RemoveTrustCenterFromFilterCommandExecute);
            this.DeleteTrustCenterFromHistoryCommand = new RelayCommand<TrustCenterMetaInfo>(this.DeleteTrustCenterFromHistoryCommandExecute);
            this.InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(this.InfoAboutTrustCenterCommandExecute);

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

        private void AddTrustCenterToFilterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            TrustCenterHistoryInactive.Remove(trustCenterMetaInfo);
            TrustCenterHistoryActive.Add(trustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();
        }

        private void RemoveTrustCenterFromFilterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            TrustCenterHistoryActive.Remove(trustCenterMetaInfo);
            TrustCenterHistoryInactive.Add(trustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();
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


            this.TrustCenterHistoryActive.Add(new TrustCenterMetaInfo(this.AddTrustCenterName, this.AddTrustCenterUrl));
            this.AddTrustCenterName = string.Empty;
            this.AddTrustCenterUrl = string.Empty;
            CertificatesCollectionView.Refresh();
        }

        private void DeleteTrustCenterFromHistoryCommandExecute(TrustCenterMetaInfo trustCenterToDelete)
        {
            this.Core.DeleteTrustCenter(trustCenterToDelete);

            this.TrustCenterHistoryActive.Remove(trustCenterToDelete);
            this.TrustCenterHistoryInactive.Remove(trustCenterToDelete);

            this.CertificatesCollectionView.Refresh();
        }

        private void InfoAboutTrustCenterCommandExecute(TrustCenterMetaInfo trustCenetrMetaInfo)
        {

            MessageBox.Show(trustCenetrMetaInfo.Name + "\n" + trustCenetrMetaInfo.TrustCenterUrl, "Information about TrustCenter", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.LoadTrustCenterHistory())
                TrustCenterHistoryActive.Add(trustCenterHistoryName);
        }

        #endregion

        private bool Filter(object obj)
        {
            if (!(obj is Certificate entry))
                return false; 

            if (!this.TrustCenterHistoryActive.Any(x => x.Name.Equals(entry.TrustCenterName)))
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
