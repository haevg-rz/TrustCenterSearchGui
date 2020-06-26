using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {
        #region fields

        private string search = "";
        private ObservableCollection<TrustCenterHistoryElement> trustCenterHistory = new ObservableCollection<TrustCenterHistoryElement>();

        #endregion

        #region Properties

        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList { get; set; }

        public ObservableCollection<TrustCenterHistoryElement> TrustCenterHistory
        {
            get => this.trustCenterHistory;
            set => base.Set(ref this.trustCenterHistory, value);
        }

        public string Search
        {
            get => this.search;
            set => base.Set(ref this.search, value);
        }

        #endregion

        #region Commands

        public RelayCommand RefreshButton { get; set; }
        public RelayCommand CollapseButton { get; set; }
        public RelayCommand AddTrustCenterButton { get; set; }

        #endregion

        public ViewModel()
        {
            SimpleIoc.Default.Register<ViewModel>();

            CertificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();


            this.RefreshButton = new RelayCommand(RefreshAndSearchCommand);
            this.AddTrustCenterButton = new RelayCommand(AddTrustCenterCommand);


        }

        #region CommandHandlings

        private void AddTrustCenterCommand()
        {
            TrustCenterHistory.Add(new TrustCenterHistoryElement("test"));
            TrustCenterHistory.Add(new TrustCenterHistoryElement("sehrsehrgroßertest"));
        }

        private void RefreshAndSearchCommand()
        {
            TrustCenterSearchGui.Core.Core.RefreshButtonCommand();

            this.ExecuteSearch();
            if (Core.Core.ConfigIsEmpty())
                MessageBox.Show("There are no TrustCenters added in the Config",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        public void ExecuteSearch()
        {
            this.CertificateSearchResultList.Clear();
            foreach (var certificate in TrustCenterSearchGui.Core.Core.Searcher(this.Search))
            {
                this.CertificateSearchResultList.Add(certificate);
            }
        }
    }
}
