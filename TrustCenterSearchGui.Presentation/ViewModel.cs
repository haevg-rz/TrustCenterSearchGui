using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    class ViewModel : ViewModelBase
    {
        public string Search 
        { 
            get => search;
            set
            {
                base.Set(ref this.search, value);
                SearchCalcuclation();
            }
        }
        private string search;

        public ObservableCollection<CertificateDatas> CertificateSearchResultList
        { 
            get => certificateSearchResultList; 
            set => base.Set(ref this.certificateSearchResultList); 
        }
        private ObservableCollection<CertificateDatas> certificateSearchResultList;

        public ViewModel()
        {



        }

        private void SearchCalcuclation()
        {
            var Search = new Core.SearchManager();
            var searchResults = Search.SearchManagerConecter(this.Search);

            this.CertificateSearchResultList = searchResults;

        }

    }
}
