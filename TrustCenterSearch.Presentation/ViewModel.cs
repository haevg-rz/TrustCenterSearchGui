﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private bool _userInputIsEnabled = true;

        public bool UserInputIsEnabled
        {
            get => this._userInputIsEnabled;
            set => base.Set(ref this._userInputIsEnabled, value);
        }

        public Core.Core Core { get; set; } = new Core.Core();

        private ICollectionView _certificatesCollectionView;

        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => Set(ref this._certificatesCollectionView, value);
        }

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryActive = new ObservableCollection<TrustCenterMetaInfo>();

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryActive
        {
            get => this._trustCenterHistoryActive;
            set => Set(ref this._trustCenterHistoryActive, value);
        }

        private ObservableCollection<TrustCenterMetaInfo> _trustCenterHistoryInactive = new ObservableCollection<TrustCenterMetaInfo>();

        public ObservableCollection<TrustCenterMetaInfo> TrustCenterHistoryInactive
        {
            get => this._trustCenterHistoryInactive;
            set => Set(ref this._trustCenterHistoryInactive, value);
        }

        private string _searchBarInput = String.Empty;

        public string SearchBarInput
        {
            get => this._searchBarInput;
            set
            {
                Set(ref this._searchBarInput, value);
                CertificatesCollectionView.Refresh();
            }
        }

        private string _addTrustCenterName = String.Empty;

        public string AddTrustCenterName
        {
            get => this._addTrustCenterName;
            set => Set(ref this._addTrustCenterName, value);
        }

        private string _addTrustCenterUrl = String.Empty;

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
        public RelayCommand<TrustCenterMetaInfo> ReloadCertificatesOfTrustCenterCommand { get; set; }
        public RelayCommand CollapseSideBarCommand { get; set; }

        private string _menuWidth = "Auto";
        public string MenuWidth
        {
            get => this._menuWidth;
            set => base.Set(ref this._menuWidth, value);
        }

        #endregion

        #region Constructor
        public ViewModel()
        {
            this.AddTrustCenterButtonCommand = new RelayCommand(this.AddTrustCenterAsyncCommandExecute);
            this.LoadDataCommand = new RelayCommand(this.LoadDataAsyncCommandExecute);
            this.AddTrustCenterToFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.AddTrustCenterToFilterCommandExecute);
            this.RemoveTrustCenterFromFilterCommand = new RelayCommand<TrustCenterMetaInfo>(this.RemoveTrustCenterFromFilterCommandExecute);
            this.DeleteTrustCenterFromHistoryCommand = new RelayCommand<TrustCenterMetaInfo>(this.DeleteTrustCenterFromHistoryCommandExecute);
            this.InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(InfoAboutTrustCenterCommandExecute);
            this.ReloadCertificatesOfTrustCenterCommand = new RelayCommand<TrustCenterMetaInfo>(this.ReloadCertificatesOfTrustCenterCommandExecute);
            this.CollapseSideBarCommand = new RelayCommand(CollapseSidebarCommandExecute);
        }

        #endregion

        #region Commandhandling

        private async void LoadDataAsyncCommandExecute()
        {
            this.UserInputIsEnabled = false;

            await this.Core.ImportAllCertificatesFromTrustCentersAsync();

            this.GetTrustCenterHistory();

            this.CertificatesCollectionView = CollectionViewSource.GetDefaultView(this.Core.GetCertificates());
            this.CertificatesCollectionView.Filter = this.Filter;

            this.UserInputIsEnabled = true;
        }

        private void CollapseSidebarCommandExecute()
        {
            if (this.MenuWidth == "Auto")
            {
                this.MenuWidth = "0";
                return;
            }

            this.MenuWidth = "Auto";
        }

        private async void ReloadCertificatesOfTrustCenterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.UserInputIsEnabled = false;

            var newTrustCenterMetaInfo = await this.Core.ReloadCertificatesOfTrustCenter(trustCenterMetaInfo);
            this.ReloadTrustCenterHistoryElement(trustCenterMetaInfo, newTrustCenterMetaInfo);

            this.UserInputIsEnabled = true;
        }

        private void ReloadTrustCenterHistoryElement(TrustCenterMetaInfo trustCenterMetaInfoToDelete,
            TrustCenterMetaInfo newTrustCenterMetaInfo)
        {
            var trustCenterHistory = new List<ObservableCollection<TrustCenterMetaInfo>> 
            {
                this.TrustCenterHistoryInactive,
                this.TrustCenterHistoryActive
            }.FirstOrDefault(x => x.Contains(trustCenterMetaInfoToDelete));

            if (trustCenterHistory == null) 
                return;

            trustCenterHistory.Remove(trustCenterMetaInfoToDelete);
            trustCenterHistory.Add(newTrustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();
        }

        private async void AddTrustCenterAsyncCommandExecute()
        {
            this.UserInputIsEnabled = false;

            TrustCenterMetaInfo newTrustCenterMetaInfo;
            try
            {
                newTrustCenterMetaInfo = await this.Core.AddTrustCenterAsync(this.AddTrustCenterName, this.AddTrustCenterUrl);
            }
            catch (ArgumentException e)
            {
                this.UserInputIsEnabled = true;
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.TrustCenterHistoryActive.Add(newTrustCenterMetaInfo);
            this.CertificatesCollectionView.Refresh();

            this.AddTrustCenterName = String.Empty;
            this.AddTrustCenterUrl = String.Empty;

            
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

        #endregion

        #region Methods

        private void GetTrustCenterHistory()
        {
            foreach (var trustCenterHistoryName in this.Core.GetTrustCenterHistory())
                this.TrustCenterHistoryActive.Add(trustCenterHistoryName);
        }

        private static void InfoAboutTrustCenterCommandExecute(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            MessageBox.Show(
                $"Name: {trustCenterMetaInfo.Name}\n{trustCenterMetaInfo.TrustCenterUrl}\nLast Update: {trustCenterMetaInfo.LastUpdate}",
                "Information about TrustCenter", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (string.IsNullOrWhiteSpace(SearchBarInput))
                return true;

            var certificateAttributes = new HashSet<string>
            {
                entry.Issuer.ToLower(),
                entry.Subject.ToLower(),
                entry.SerialNumber,
                entry.NotBefore,
                entry.NotAfter,
                entry.Thumbprint.ToLower(),
                entry.PublicKeyLength
                
            };

            return certificateAttributes.Any(atr => atr.Contains(this.SearchBarInput.ToLower()));
        }

        #endregion
    }
}