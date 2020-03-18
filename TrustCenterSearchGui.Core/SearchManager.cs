using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class SearchManager
    {

        
        public ObservableCollection<CertificateDatas> SearchManagerConecter(string search)
        {
            var searchResults = new ObservableCollection<CertificateDatas>();
            {
                new CertificateDatas() { Data1 = "Test", Data2 = "TestZwei", Data3 = "TestDrei", Data4 = "TestVier", Data5 = "TestFünf", Data6 = "TestSechs" };
                new CertificateDatas() { Data1 = "Test2", Data2 = "TestZwei", Data3 = "TestDrei", Data4 = "TestVier", Data5 = "TestFünf", Data6 = "TestSechs" };
                new CertificateDatas() { Data1 = "Test", Data2 = "TestZwei", Data3 = "TestDrei", Data4 = "TestVier", Data5 = "TestFünf", Data6 = "TestSechs" };
            };

            return searchResults;
        }

    }
}
