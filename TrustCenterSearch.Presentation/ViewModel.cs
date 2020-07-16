using System;
using System.Collections.Generic;
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
        #region Properties

        private bool _userImputIsEnablet = true;

        public bool UserInputIsEnabled
        {
            get => this._userImputIsEnablet;
            set => base.Set(ref this._userImputIsEnablet, value);
        }

        public Core.Core Core { get; set; }

        private ICollectionView _certificatesCollectionView;

        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => base.Set(ref this._certificatesCollectionView, value);
        }

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryActive = new ObservableCollection<TrustCenterMetaInfo>();

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryActive
        {
            get => this._trustCenterHistoryActive;
            set => base.Set(ref this._trustCenterHistoryActive, value);
        }

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryInactive = new ObservableCollection<TrustCenterMetaInfo>();

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryInactive
        {
            get => this._trustCenterHistoryInactive;
            set => base.Set(ref this._trustCenterHistoryInactive, value);
        }

        private string _searchBarInput = String.Empty;

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set
            {
                base.Set(ref this._searchBarInput, value);
                this.CertificatesCollectionView.Refresh();
            }
        }

        private string _addTrustCenterName = String.Empty;

        public string AddTrustCenterName
        {
            get => this._addTrustCenterName;
            set => base.Set(ref this._addTrustCenterName, value);
        }

        private string _addTrustCenterUrl = String.Empty;

        public string AddTrustCenterUrl
        {
            get => this._addTrustCenterUrl;
            set => base.Set(ref this._addTrustCenterUrl, value);
        }
        #endregion

        #region Commands
        public RelayCommand AddTrustCenterButtonCommand { get; set; }
        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> AddTrustCenterToFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> RemoveTrustCenterFromFilterCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> DeleteTrustCenterFromHistoryCommand { get; set; }
        public RelayCommand<TrustCenterMetaInfo> InfoAboutTrustCenterCommand { get; set; }


        #endregion

        #region Constructor
        public ViewModel()
        {
            this.AddTrustCenterButtonCommand = new RelayCommand(this.AddTrustCenterCommandExecute);
            this.LoadDataCommand = new RelayCommand(this.LoadDataCommandExecute);
            this.AddTrustCenterToFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.AddTrustCenterToFilterCommandExecute);
            this.RemoveTrustCenterFromFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.RemoveTrustCenterFromFilterCommandExecute);
            this.DeleteTrustCenterFromHistoryCommand = new RelayCommand<TrustCenterMetaInfo>(this.DeleteTrustCenterFromHistoryCommandExecute);
            this.InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(InfoAboutTrustCenterCommandExecute);

            this.Core = new Core.Core();
        }

        #endregion

        #region Commandhandling

        private async void LoadDataCommandExecute()
        {
            this.UserInputIsEnabled = false;

            await this.Core.ImportAllCertificatesFromTrustCenters();

            this.GetTrustCenterHistory();

            var defaultView = CollectionViewSource.GetDefaultView(this.Core.GetCertificates());
            defaultView.Filter = this.Filter;
            this.CertificatesCollectionView = defaultView;

            this.UserInputIsEnabled = true;
        }

        private async void AddTrustCenterCommandExecute()
        {
            this.UserInputIsEnabled = false;
            try
            {
                await Core.AddTrustCenter(this.AddTrustCenterName, this.AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                this.UserInputIsEnabled = true;
                return;
            }

            this.TrustCenterHistoryActive.Add(new TrustCenterMetaInfo(this.AddTrustCenterName, this.AddTrustCenterUrl));
            this.AddTrustCenterName = String.Empty;
            this.AddTrustCenterUrl = String.Empty;
            CertificatesCollectionView.Refresh();
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

        private static void InfoAboutTrustCenterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            MessageBox.Show(trustCenterMetaInfo.Name + "\n" + trustCenterMetaInfo.TrustCenterUrl, "Information about TrustCenter", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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

        #endregion

        #region Methods

        private void GetTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.GetTrustCenterHistory())
                TrustCenterHistoryActive.Add(trustCenterHistoryName);
        }

        private bool Filter(object obj)
        {
            if (!(obj is Certificate entry))
                return false;

            bool IsEnabled(Certificate certificate)
            {
                return this.TrustCenterHistoryActive.Any(x => x.Name.Equals(certificate.TrustCenterName));
            }

            if (!IsEnabled(entry))
                return false;

            if (string.IsNullOrWhiteSpace(this.SearchBarInput))
                return true;

            var certificateAttributes = new HashSet<string>
            {
                entry.Issuer.ToLower(),
                entry.Subject.ToLower(),
                entry.SerialNumber.ToLower(),
                entry.NotBefore.ToLower(),
                entry.NotAfter.ToLower(),
                entry.Thumbprint.ToLower()
            };

            return certificateAttributes.Any(atr => atr.Contains(SearchBarInput.ToLower()));
        }
        #endregion
    }
}
