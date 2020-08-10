using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSamples;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchPresentation.Test
{
    class FilterTestData : IEnumerable<object[]>
    {

        public static Task<IEnumerable<Certificate>> ProvideTaskIEnumerableCertificate()
        {
            async Task<IEnumerable<Certificate>> ProvideSampleOfCertificatesAsync()
            {
                return Samples.ProvideSampleCertificates();
            }

            return ProvideSampleOfCertificatesAsync();
        }

        private readonly List<object[]> filterTestData = new List<object[]>
        {
            new object[] {null, false,null},
            new object[] {Samples.ProvideSingleSampleCertificate(), false,null},
            new object[] {Samples.ProvideSampleCertificates().First(), true,null},
            new object[] {Samples.ProvideSampleCertificates().First(), true,"Sample1"},
            new object[] {Samples.ProvideSampleCertificates().First(),true, "01.01.2" },
            new object[] {Samples.ProvideSampleCertificates().First(),false, "01.01.22123" }
        };

        public IEnumerator<object[]> GetEnumerator() => filterTestData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
