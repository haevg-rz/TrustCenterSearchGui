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

                var certificate = new List<string>();
                certificate.Add("");

                var certificateCounter = 0;

                for (var i = 0; i < str.Length; i++)
                    if (str[i] != "")
                        certificate[certificateCounter] += str[i];
                    else
                    {
                        certificateCounter++;
                        certificate.Add("");
                    }

                var certificatsStr = (from inhaltW in certificate
                        where inhaltW != ""
                        select new X509Certificate2(Convert.FromBase64String(inhaltW))
                        into s
                        select Convert.ToString(s))
                       .ToList();

                foreach (var certificat in certificatsStr)
                {
                    var certificateWithoutSpaces = System.Text.RegularExpressions.Regex.Split(certificat, @"\n");

                    var certificatInOneString = certificateWithoutSpaces.Aggregate("", (current, s) => current + s);

                    certificateWithoutSpaces = System.Text.RegularExpressions.Regex.Split(certificatInOneString, @"\r");

                    certificates.Add(new Certificate()
                    { 
                        Subject = (certificateWithoutSpaces[1]),
                        Issuer = (certificateWithoutSpaces[4]),
                        SerialNumber = (certificateWithoutSpaces[7]),
                        NotAfter = Convert.ToDateTime(certificateWithoutSpaces[10]),
                        NotBefore = Convert.ToDateTime(certificateWithoutSpaces[13]),
                        Thumbprint = (certificateWithoutSpaces[16])
                    });
                }
            }

            return certificates;
        }

        public void SetTimeStamp(string filePath)
        {
            CreateMissingPath(filePath);

            var timeStamp = DateTime.Now;
            File.WriteAllText(filePath + "TimeStamp.JSON", Convert.ToString(timeStamp));
        }

        public void CreateMissingPath(string dataPath)
        {
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
        }
    }
}