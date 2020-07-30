using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchCore.Test
{
    public static class Samples
    {
        public static object Subject { get; private set; }

        public static List<Certificate> ProvideSampleCertificates()
        {
            var sample = new List<Certificate>()
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
            return sample;
        }

        public static Config ProvideSampleConfig()
        {
            var sample = new Config()
            {
                TrustCenterMetaInfos = Samples.ProvideSampleMetaInfos()
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

        //public static byte[] ProvidSampleByteArrays()
        //{
        //    //var enc = new System.Text.ASCIIEncoding();

        //    //var cer = ProvideSampleCertificates().Last();
        //    //var certifgicate = new X509Certificate();
        //    //var strings1 = Convert.ToString(cer);
        //    //return enc.GetBytes(strings);

        //}
    }
}