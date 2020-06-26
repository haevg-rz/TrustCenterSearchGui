using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class DataManager
    {
        public List<Certificate> GetCertificatesFromAppData(Config config, string filePath)
        {
            var certificates = new List<Certificate>();

            foreach (var trustCenter in config.TrustCenters)
            {
                var textFromTrustCenter = File.ReadAllText(filePath + trustCenter.Name + @".txt");
                var certificatesInTrustCenter = GetCertificatesFromString(textFromTrustCenter);
                certificates.AddRange(certificatesInTrustCenter);
            }
            return certificates;
        }

        private static IEnumerable<Certificate> GetCertificatesFromString(string textFromTrustCenter)
        {
            var certificatesTxt = textFromTrustCenter.Split(new [] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
            
            var cer = (from certificateTxt in certificatesTxt
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
        }

        public void SetTimeStamp(string filePath)
        {
            CreateDirectoryIfMissing(filePath);

            var timeStamp = DateTime.Now;
            File.WriteAllText(filePath + "TimeStamp.json", Convert.ToString(timeStamp));
        }

        public void CreateDirectoryIfMissing(string dataPath)
        {
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
        }
    }
}