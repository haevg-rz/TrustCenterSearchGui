using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Models;
using TrustCenterSearch.Presentation.Models;

namespace TrustCenterSearch.Presentation
{
    public class ViewModel : ViewModelBase
    {
        #region Properties

        public Core.Core Core { get; set; } = new Core.Core();

        private bool _userInputIsEnabled = true;
        public bool UserInputIsEnabled
        {
            get => this._userInputIsEnabled;
            set => base.Set(ref this._userInputIsEnabled, value);
        }

        public List<TrustCenterHistoryElement> TrustCenterHistory { get; set; } = new List<TrustCenterHistoryElement>();

        private ICollectionView _trustCenterHistoryCollectionView;

        public ICollectionView TrustCenterHistoryCollectionView
        {
            get => this._trustCenterHistoryCollectionView;
            set => Set(() => this.TrustCenterHistoryCollectionView, ref this._trustCenterHistoryCollectionView, value);
        }

        private ICollectionView _certificatesCollectionView;
        public ICollectionView CertificatesCollectionView
        {
            get => this._certificatesCollectionView;
            set => Set(ref this._certificatesCollectionView, value);
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
        public RelayCommand<TrustCenterHistoryElement> ApplyTrustCenterHistoryFilterCommand { get; set; }
        public RelayCommand<TrustCenterHistoryElement> DeleteTrustCenterFromHistoryCommand { get; set; }
        public RelayCommand<TrustCenterHistoryElement> InfoAboutTrustCenterCommand { get; set; }
        public RelayCommand<TrustCenterHistoryElement> ReloadCertificatesOfTrustCenterCommand { get; set; }
        public RelayCommand CollapseSideBarCommand { get; set; }
        public RelayCommand OpenWikiWebpageCommand { get; set; }
        public RelayCommand<Certificate> CopyToClipboardCommand { get; set; }
        public RelayCommand OpenConfigCommand { get; set; }

        private string _menuWidth = "Auto";
        private readonly string _gitHubWikiUrl = "https://github.com/haevg-rz/TrustCenterSearchGui/wiki";
        private readonly string _configFolderPath =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch\Config.JSON";

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
            this.ApplyTrustCenterHistoryFilterCommand = new RelayCommand<TrustCenterHistoryElement>(this.ApplyTrustCenterHistoryFilterCommandExecute);
            this.DeleteTrustCenterFromHistoryCommand = new RelayCommand<TrustCenterHistoryElement>(this.DeleteTrustCenterFromHistoryCommandExecute);
            this.InfoAboutTrustCenterCommand = new RelayCommand<TrustCenterHistoryElement>(InfoAboutTrustCenterCommandExecute);
            this.ReloadCertificatesOfTrustCenterCommand = new RelayCommand<TrustCenterHistoryElement>(this.ReloadCertificatesOfTrustCenterCommandExecute);
            this.CollapseSideBarCommand = new RelayCommand(CollapseSidebarCommandExecute);
            this.OpenWikiWebpageCommand = new RelayCommand(OpenWikiWebpageCommandExecute);
            this.CopyToClipboardCommand = new RelayCommand<Certificate>(this.CopySearchResultToClipboardCommandExecute);
            this.OpenConfigCommand = new RelayCommand(this.OpenConfigCommandExecute);
        }

        #endregion

        #region Commandhandling
        private void CopySearchResultToClipboardCommandExecute(Certificate certificate)
        {
            var jsonString = JsonConvert.SerializeObject(certificate,Formatting.Indented);
            Clipboard.SetText(jsonString);
        }
        private async void LoadDataAsyncCommandExecute()
        {
            this.UserInputIsEnabled = false;

            await this.Core.ImportAllCertificatesFromTrustCentersAsync();

            this.TrustCenterHistoryCollectionView = CollectionViewSource.GetDefaultView(this.GetTrustCenterHistory());
            this.TrustCenterHistoryCollectionView.SortDescriptions.Add(new SortDescription(nameof(TrustCenterHistoryElement.Active), ListSortDirection.Descending));

            this.CertificatesCollectionView = CollectionViewSource.GetDefaultView(this.Core.GetCertificates());
            this.CertificatesCollectionView.Filter = this.Filter;

            this.UserInputIsEnabled = true;
        }

        private void CollapseSidebarCommandExecute()
        {
            this.MenuWidth = this.MenuWidth.Equals("Auto") ? "0" : this.MenuWidth = "Auto";
        }

        private async void ReloadCertificatesOfTrustCenterCommandExecute(TrustCenterHistoryElement trustCenterMetaInfo)
        {
            this.UserInputIsEnabled = false;

            await this.Core.ReloadCertificatesOfTrustCenter(trustCenterMetaInfo.TrustCenterMetaInfo);
            this.GetTrustCenterHistory();

            RefreshCollectionViews();

            this.UserInputIsEnabled = true;
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

            this.TrustCenterHistory.Add(new TrustCenterHistoryElement(newTrustCenterMetaInfo));

            RefreshCollectionViews();

            this.AddTrustCenterName = String.Empty;
            this.AddTrustCenterUrl = String.Empty;

            this.UserInputIsEnabled = true;
        }



        private void DeleteTrustCenterFromHistoryCommandExecute(TrustCenterHistoryElement trustCenterToDelete)
        {
            var deleteConfirmation = MessageBox.Show("Are you sure you want to delete this Trust Center?", "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (!deleteConfirmation.Equals(MessageBoxResult.OK)) return;

            this.UserInputIsEnabled = false;
            this.Core.DeleteTrustCenter(trustCenterToDelete.TrustCenterMetaInfo);

            this.TrustCenterHistory.Remove(trustCenterToDelete);

            RefreshCollectionViews();

            this.UserInputIsEnabled = true;
        }

        private void ApplyTrustCenterHistoryFilterCommandExecute(TrustCenterHistoryElement trustCenterHistoryElement)
        {
            trustCenterHistoryElement.Active = !trustCenterHistoryElement.Active;
            this.TrustCenterHistoryCollectionView.Refresh();
            this.CertificatesCollectionView.Refresh();
        }

        private void OpenWikiWebpageCommandExecute()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/c start {this._gitHubWikiUrl}"
            };
            Process.Start(psi);
        }

        private void OpenConfigCommandExecute()
        {
            var psi = new ProcessStartInfo
            {
                FileName = this._configFolderPath,
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(psi);
        }

        #endregion

        #region Methods

        private List<TrustCenterHistoryElement> GetTrustCenterHistory()
        {
            this.TrustCenterHistory.Clear();
            foreach (var trustCenterMetaInfo in this.Core.GetTrustCenterHistory())
                TrustCenterHistory.Add(new TrustCenterHistoryElement(trustCenterMetaInfo));
            return TrustCenterHistory;
        }

        private static void InfoAboutTrustCenterCommandExecute(TrustCenterHistoryElement trustCenterMetaInfo)
        {
            MessageBox.Show(
                $"Name: {trustCenterMetaInfo.TrustCenterMetaInfo.Name}\n{trustCenterMetaInfo.TrustCenterMetaInfo.TrustCenterUrl}\nLast Update: {trustCenterMetaInfo.TrustCenterMetaInfo.LastUpdate}",
                "Information about TrustCenter", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool Filter(object obj)
        {
            if (!(obj is Certificate entry))
                return false;

            bool IsEnabled(Certificate certificate)
            {
                return this.TrustCenterHistory.Any(x => x.TrustCenterMetaInfo.Name.Equals(certificate.TrustCenterName) && x.Active.Equals(true));
            }

            if (TrustCenterHistory.Count > 0)
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

        private void RefreshCollectionViews()
        {
            this.TrustCenterHistoryCollectionView.Refresh();
            this.CertificatesCollectionView.Refresh();
        }

        #endregion
    }
}