using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class SearchManager
    {

        
        public ObservableCollection<CertificateData> SearchManagerConecter(string search)
        {
            var searchResults = new ObservableCollection<CertificateData>() { new CertificateData { Data1 = "asda", Data2 = "asodiaslk", Data3 = "asd", Data4 = "sad", Data5 = "asd", Data6 = "asd" } };

            return searchResults;
        }

    }
}
