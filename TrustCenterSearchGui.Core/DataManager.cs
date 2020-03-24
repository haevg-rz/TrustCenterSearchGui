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
        private static string DataPathTimeStamp { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            @"\TrustCenterSearch\data\TimeStamp.txt";

        private static string DataPathDownloadedData { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            @"\TrustCenterSearch\data\";


        public List<Certificate> GetCertificateFromAppData(Config config)
        {
            var certificates = new List<Certificate>();

            foreach (var trustCenter in config.TrustCenters)
            {
                var str = File.ReadAllLines(DataPathDownloadedData + trustCenter.Name + @".txt");

                var certificate = new List<string>();
                certificate.Add("");

                var certificateCounter = 0;

                for (var i = 0; i < str.Length; i++)
                    if (str[i] != "")
                    {
                        certificate[certificateCounter] += str[i];
                    }
                    else
                    {
                        certificateCounter++;
                        certificate.Add("");
                    }

                var certificats = (from inhaltW in certificate
                        where inhaltW != ""
                        select new X509Certificate2(Convert.FromBase64String(inhaltW))
                        into s
                        select Convert.ToString(s))
                       .ToList();

                foreach (var certificat in certificats)
                {
                    var certificateWithoutSpaces = System.Text.RegularExpressions.Regex.Split(certificat, @"\n");
                    var certificateWithoutSpacesTogether = "";

                    foreach (var s in certificateWithoutSpaces) certificateWithoutSpacesTogether += s;

                    var certificateOrded = System.Text.RegularExpressions.Regex.Split(certificateWithoutSpacesTogether, @"\r");

                    certificates.Add(new Certificate()
                    { 
                        Subject = (certificateOrded[1]),
                        Issuer = (certificateOrded[4]),
                        SerialNumber = (certificateOrded[7]),
                        NotAfter = Convert.ToDateTime(certificateOrded[10]),
                        NotBefore = Convert.ToDateTime(certificateOrded[13]),
                        Thumbprint = (certificateOrded[16])
                    });
                }
            }

            return certificates;
        }

        public DateTime GetTimeStamp()
        {
            var str = File.ReadAllText(DataPathTimeStamp);
            var timeStamp = Convert.ToDateTime(str);
            return timeStamp;
        }

        public void SetTimeStamp()
        {
            CreateMissingPath(DataPathTimeStamp);

            var timeStamp = DateTime.Now;
            File.WriteAllText(DataPathTimeStamp, Convert.ToString(timeStamp));
        }

        public void CreateMissingPath(string dataPath)
        {
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
        }
    }
}