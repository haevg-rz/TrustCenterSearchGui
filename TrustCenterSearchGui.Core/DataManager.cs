using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class DataManager
    {
        public List<Certificate> GetCertificateFromAppData(Config config, string filePath)
        {
            var certificates = new List<Certificate>();

            foreach (var trustCenter in config.TrustCenters)
            {
                var str = File.ReadAllLines(filePath + trustCenter.Name + @".txt");

                var certificate = new List<string> {""};
                var certificateCounter = 0;

                foreach (var s in str)
                    if (s != "")
                        certificate[certificateCounter] += s;
                    else
                    {
                        certificateCounter++;
                        certificate.Add("");
                    }

                var cer = (from c in certificate
                                      where c != ""
                                      select new X509Certificate2(Convert.FromBase64String(c)));

                certificates.AddRange(cer.Select(c => new Certificate()
                {
                    Subject = c.Subject,
                    Issuer = c.Issuer,
                    SerialNumber = c.SerialNumber,
                    NotAfter = c.NotAfter,
                    NotBefore = c.NotBefore,
                    Thumbprint = c.Thumbprint
                }));
            }

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