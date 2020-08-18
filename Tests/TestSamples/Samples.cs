using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;
using TrustCenterSearch.Presentation.Models;

namespace TestSamples
{
    public class Samples 
    {
        public static List<Certificate> ProvideSampleCertificates()
        {

            var sample = new List<Certificate>()
            {
                new Certificate()
                {
                    NotAfter = "01.01.2020",
                    NotBefore = "01.01.2023",
                    TrustCenterName = "Sample1",
                    SerialNumber = "Sample1SN",
                    PublicKeyLength = "2048",
                    Subject = "CN=Sample1CN OU=Sample1OU1 OU=Sample1OU2",
                    Thumbprint = "Sample1Thumbprint"
                },
                new Certificate()
                {
                    NotAfter = "01.01.2022",
                    NotBefore = "01.01.2025",
                    TrustCenterName = "Sample1",
                    SerialNumber = "Sample2SN",
                    PublicKeyLength = "2048",
                    Subject = "CN=Sample2CN OU=Sample2OU1 OU=Sample2OU2",
                    Thumbprint = "Sample2Thumbprint"
                },
                new Certificate()
                {
                    NotAfter = "01.01.2020",
                    NotBefore = "01.01.2023",
                    TrustCenterName = "Sample2",
                    SerialNumber = "Sample3SN",
                    PublicKeyLength = "2048",
                    Subject = "CN=Sample3CN OU=Sample3OU1 OU=Sample3OU2",
                    Thumbprint = "Sample3Thumbprint"
                }
            };

            return sample;
        }

        public static Certificate ProvideSingleSampleCertificate()
        {

            var sample = new Certificate()
            {
                NotAfter = "01.01.2020",
                NotBefore = "01.01.2023",
                TrustCenterName = "Sample4",
                SerialNumber = "Sample1SN",
                PublicKeyLength = "2048",
                Subject = "CN=Sample1CN OU=Sample1OU1 OU=Sample1OU2",
                Thumbprint = "Sample1Thumbprint"
            };
               
            return sample;
        }

        public static List<TrustCenterHistoryElement> ProvideSampleListOfTrustCenterHistoryElements()
        {
            var sample = new List<TrustCenterHistoryElement>()
            {
                {
                    new TrustCenterHistoryElement(new TrustCenterMetaInfo("Sample1", "abc",
                        new DateTime(2020, 1, 1)))
                },
                {
                   new TrustCenterHistoryElement(new TrustCenterMetaInfo("Sample2", "abc",
                        new DateTime(2020, 1, 1)))
                },
                {
                    new TrustCenterHistoryElement(new TrustCenterMetaInfo("Sample3", "abc",
                        new DateTime(2020, 1, 1)))
                }
            };
            return sample;
            

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

        public static Task<byte[]> ProvideSampleTaskByteArray()
        {
            async Task<byte[]> GetByteArray()
            {
                return new byte[0];
            }

            return GetByteArray();
        }

        public static Task<IEnumerable<Certificate>> ProvideTaskIEnumerableCertificate()
        {
            async Task<IEnumerable<Certificate>> ProvideSampleOfCertificatesAsync()
            {
                return ProvideSampleCertificates();
            }

            return ProvideSampleOfCertificatesAsync();
        }

        public static Task<List<Certificate>> ProvideTaskIListCertificate()
        {
            async Task<List<Certificate>> ProvideSampleOfCertificatesAsync()
            {
                return ProvideSampleCertificates();
            }

            return ProvideSampleOfCertificatesAsync();
        }

    }
}
