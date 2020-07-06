using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement
{
    internal class ImportManager
    {
        internal async Task<List<Certificate>> ImportCertificatesFromDownloadedTrustCenter(
            List<Certificate> certificates, TrustCenter trustCenter, string filePath)
        {
            var certificatesTxt = await ReadFileAsync(trustCenter, filePath);

            var cer = from certificateTxt in certificatesTxt
                select new X509Certificate2(Convert.FromBase64String(certificateTxt));

            certificates.AddRange(cer.Select(c => new Certificate()
            {
                Subject = c.Subject,
                Issuer = c.Issuer,
                SerialNumber = c.SerialNumber,
                NotAfter = c.NotAfter,
                NotBefore = c.NotBefore,
                Thumbprint = c.Thumbprint
            }).AsParallel());

            return certificates;
        }

        private static async Task<string[]> ReadFileAsync(TrustCenter trustCenter, string filePath)
        {
            using (var reader = File.OpenText(filePath + trustCenter.Name + @".txt"))
            {
                var fileText = await reader.ReadToEndAsync();
                return fileText.Split(new[] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }


        /*internal List<Certificate> ImportCertificatesFromDownloadedTrustCenter(Config config, string filePath)
        {
            var certificates = new List<Certificate>();

            foreach (var certificatesInTrustCenter in config.TrustCenters.
                    Select(trustCenter => File.ReadAllText(filePath + trustCenter.Name + @".txt")).
                    Select(GetCertificatesFromString).AsParallel())
                certificates.AddRange(certificatesInTrustCenter);

            return certificates;
        }

        private static IEnumerable<Certificate> GetCertificatesFromString(string textFromTrustCenter)
        {
            var certificatesTxt = textFromTrustCenter.Split(new [] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
            
            var cer = (from certificateTxt in certificatesTxt.AsParallel()
                                                select new X509Certificate2(Convert.FromBase64String(certificateTxt)));
            
            var certificates = new List<Certificate>();
            
            certificates.AddRange(cer.Select(c => new Certificate()
                {
                    Subject = c.Subject,
                    Issuer = c.Issuer,
                    SerialNumber = c.SerialNumber,
                    NotAfter = c.NotAfter,
                    NotBefore = c.NotBefore,
                    Thumbprint = c.Thumbprint
                }));

            return certificates;
        }*/

        internal void SetTimeStamp(string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var timeStamp = DateTime.Now;
            File.WriteAllText(filePath + "TimeStamp.json", Convert.ToString(timeStamp));
        }
    }
}