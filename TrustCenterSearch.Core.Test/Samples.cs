using System.Collections.Generic;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchCore.Test
{
    public static class Samples
    {
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
    }
}