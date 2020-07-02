using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TrustCenterSearch.Core.Models;

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

        public ObservableCollection<Certificate> DisplayedCertificates { get; set; }

        public ICollectionView CollectionView { get; set; }

        public ObservableCollection<string> TrustCenterHistory
        {
            get => this._trustCenterHistory;
            set => base.Set(ref this._trustCenterHistory, value);
        }

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set
            {
                base.Set(ref this._searchBarInput, value);
                this.CollectionView.Refresh();
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

            DisplayedCertificates = new ObservableCollection<Certificate>();
            this.AddTrustCenterButton = new RelayCommand(AddTrustCenter);

            this.LoadTrustCenterHistory();
            this.LoadCertificates();

            var collectionView = CollectionViewSource.GetDefaultView(this.DisplayedCertificates);
            collectionView.Filter = this.Filter;
            this.CollectionView = collectionView;
        }

        #region TrustCenterSearchManager Interface

        private void LoadCertificates()
        {
            foreach (var certificate in Core.LoadCertificates())
            {
                DisplayedCertificates.Add(certificate);
            }
        }

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
            
            LoadCertificates();
        }

        private void LoadTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in Core.LoadTrustCenterHistory())
                TrustCenterHistory.Add(trustCenterHistoryName);
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
