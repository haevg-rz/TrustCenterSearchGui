using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            //if (base.IsInDesignMode)
            //{
            //    CertificateSearchResultList.Add(new SearchResultsAndBorder());
            //    {
                    
                    
            //    } 
            //}

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
            var Search = new Core.SearchManager();
            var searchResults = Search.MainSearchManager(this.Search);

            this.CertificateSearchResultList = searchResults;

        }

    }
}
