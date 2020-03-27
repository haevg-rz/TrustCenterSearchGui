using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {
        public RelayCommand RefreshButton { get; set; }

        public ViewModel()
        {
            this.RefreshButton = new RelayCommand(TrustCenterSearchGui.Core.Intersection.RefreshButton);
            
            this.SearchCalcuclation();
        }

        private string search;
        public string Search 
        { 
            get => this.search;
            set
            {
                base.Set(ref this.search, value);
                this.SearchCalcuclation();
            }
        }

        private ObservableCollection<SearchResultsAndBorder> certificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();
        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList
        { 
            get => this.certificateSearchResultList; 
            set => base.Set(ref this.certificateSearchResultList, value);
        }

        private void SearchCalcuclation()
        {
            var searchResult = TrustCenterSearchGui.Core.Intersection.Searcher(this.Search);

            this.CertificateSearchResultList = searchResult;
        }
    }
}
