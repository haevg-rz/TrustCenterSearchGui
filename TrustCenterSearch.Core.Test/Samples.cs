using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchCore.Test
{
    public class Samples : IEnumerable<object[]>
    {
        public static List<Certificate> CerSamples = new List<Certificate>()
        {
            new Certificate()
            {
                NotAfter = "01.01.2020",
                NotBefore = "01.01.2023",
                TrustCenterName = "SampleTrustCenter1",
                SerialNumber = "Sample1SN",
                PublicKeyLength = "2048",
                Subject = "CN=Sample1CN OU=Sample1OU1 OU=Sample1OU2",
                Thumbprint = "Sample1Thumbprint"
            },
            new Certificate()
            {
                NotAfter = "01.01.2022",
                NotBefore = "01.01.2025",
                TrustCenterName = "SampleTrustCenter1",
                SerialNumber = "Sample2SN",
                PublicKeyLength = "2048",
                Subject = "CN=Sample2CN OU=Sample2OU1 OU=Sample2OU2",
                Thumbprint = "Sample2Thumbprint"
            },
            new Certificate()
            {
                NotAfter = "01.01.2020",
                NotBefore = "01.01.2023",
                TrustCenterName = "SampleTrustCenter2",
                SerialNumber = "Sample3SN",
                PublicKeyLength = "2048",
                Subject = "CN=Sample3CN OU=Sample3OU1 OU=Sample3OU2",
                Thumbprint = "Sample3Thumbprint"
            }
        };

        internal static IEnumerable<Certificate> ProvideOneSampleCertificate()
        {
            throw new NotImplementedException();
        }

        public static Config ProvideSampleConfig()
        {
            var sample = new Config()
            {
                TrustCenterMetaInfos = ProvideSampleMetaInfos()
            };
            return sample;
        }

        public static List<TrustCenterMetaInfo> ProvideSampleMetaInfos()
        {
            var sample = new List<TrustCenterMetaInfo>()
            {
                {
                    new TrustCenterMetaInfo("Sample1", "https://www.speedtestx.de/testfiles/data_1mb.test",
                        new DateTime(2020, 1, 1))
                },
                {
                    new TrustCenterMetaInfo("Sample2", "https://www.speedtestx.de/testfiles/data_1mb.test",
                        new DateTime(2020, 1, 1))
                },
                {
                    new TrustCenterMetaInfo("Sample3", "https://www.speedtestx.de/testfiles/data_1mb.test",
                        new DateTime(2020, 1, 1))
                }
            };
            return sample;
        }

        private readonly List<object[]> filterTestData = new List<object[]>
        {
            new object[] {null, false},
            new object[] {CerSamples.First(), true}

        };

        public IEnumerator<object[]> GetEnumerator() => filterTestData.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}