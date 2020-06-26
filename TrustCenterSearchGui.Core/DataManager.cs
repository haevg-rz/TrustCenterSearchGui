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
                 
                var certificatesInTrustCenter = new List<Certificate>();
                certificatesInTrustCenter = GetCertificateFromString(textFromTrustCenter));

                foreach (var certificate in certificatesInTrustCenter)
                {
                    certificates.AddRange(certificates.Select(c => new Certificate()
                    {
                        Subject = c.Subject,
                        Issuer = c.Issuer,
                        SerialNumber = c.SerialNumber,
                        NotAfter = c.NotAfter,
                        NotBefore = c.NotBefore,
                        Thumbprint = c.Thumbprint
                    }));
                }
            }
            return certificates;
        }

        private List<Certificate> GetCertificateFromString(string textFromTrustCenter)
        {
            var certificatesRaw = textFromTrustCenter.Split(new string[] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);

            var cer = (from certificateRaw in certificatesRaw
                    select new X509Certificate2(Convert.FromBase64String(certificateRaw)));

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
            CreateMissingPath(filePath);

            var timeStamp = DateTime.Now;
            File.WriteAllText(filePath + "TimeStamp.json", Convert.ToString(timeStamp));
        }

        public void CreateMissingPath(string dataPath)
        {
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
        }
    }
}