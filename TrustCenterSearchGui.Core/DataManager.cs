using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    class DataManager
    {
        private static string DataPathTimeStamp { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                  @"\TrustCenterSearch\Data\TimeStamp.txt";
        private static string DataPathDownloadedData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                  @"\TrustCenterSearch\Data\";


        public List<Certificate> GetCertificateFromAppData(Config config)
        {  
            var certificates = new List<Certificate>();

            foreach (var trustCenter in config.TrustCenters)
            {
                var str = System.IO.File.ReadAllText(DataPathDownloadedData + trustCenter.Name);

                var cert = new X509Certificate2(Convert.FromBase64String(str));

                //cert in new Certificate(); 

                certificates.Add(new Certificate());
            }

            return certificates;
        }

        public DateTime GetCurrentTimeStamp()
        {
            var str = System.IO.File.ReadAllText(DataPathTimeStamp);
            var timeStamp = Convert.ToDateTime(str);
            return timeStamp;
        }

        public void RefreshDateTime()
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
