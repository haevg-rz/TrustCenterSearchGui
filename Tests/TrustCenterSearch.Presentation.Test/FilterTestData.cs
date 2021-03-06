﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSamples;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Presentation.Test
{
    class FilterTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> filterTestData = new List<object[]>
        {
            new object[] {null, false,null},
            new object[] {Samples.ProvideSingleSampleCertificate(), false,null},
            new object[] {Samples.ProvideSampleCertificates().First(), true,null},
            new object[] {Samples.ProvideSampleCertificates().First(), true,"Sample1"},
            new object[] {Samples.ProvideSampleCertificates().First(),true, "01.01.2" },
            new object[] {Samples.ProvideSampleCertificates().First(),false, "01.01.22123" }
        };

        public IEnumerator<object[]> GetEnumerator() => this.filterTestData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
