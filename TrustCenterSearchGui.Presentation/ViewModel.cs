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

        public ObservableCollection<CertificateData> CertificateSearchResultList
        { 
            get => this.certificateSearchResultList; 
            set => base.Set(ref this.certificateSearchResultList, value);
        }
        private ObservableCollection<CertificateData> certificateSearchResultList = new ObservableCollection<CertificateData>();

        public ViewModel()
        {
            this.CertificateSearchResultList.Add(new CertificateData() { Data1 = "test zertifikat 1", Data2 = "1651", Data3 = "asda", Data4 = "test zertifikat 1", Data5 = "1651", Data6 = "asda" });
            this.CertificateSearchResultList.Add(new CertificateData() { Data1 = "test zertifikat 1", Data2 = "1651", Data3 = "asda", Data4 = "test zertifikat 1", Data5 = "1651", Data6 = "asda" });
            this.CertificateSearchResultList.Add(new CertificateData() { Data1 = "test zertifikat 1", Data2 = "1651", Data3 = "asda", Data4 = "test zertifikat 1", Data5 = "1651", Data6 = "asda" });
            this.CertificateSearchResultList.Add(new CertificateData() { Data1 = "test zertifikat 1", Data2 = "1651", Data3 = "asda", Data4 = "test zertifikat 1", Data5 = "1651", Data6 = "asda" });
            this.CertificateSearchResultList.Add(new CertificateData() { Data1 = "test zertifikat 1", Data2 = "1651", Data3 = "asda", Data4 = "test zertifikat 1", Data5 = "1651", Data6 = "asda" });
        }

        private void SearchCalcuclation()
        {
            var Search = new Core.SearchManager();
            var searchResults = Search.SearchManagerConecter(this.Search);

            this.CertificateSearchResultList = searchResults;
        }

    }
}
