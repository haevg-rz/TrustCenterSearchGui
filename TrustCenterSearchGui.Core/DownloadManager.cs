using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace TrustCenterSearchGui.Core
{
    public class DownloadManager
    {
        private static string DataPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                  @"\TrustCenterSearch\Data\";

        public void DownloadDataFromConfic(Config config)
        {
            CreateMissingPath();
            var client = new WebClient();

            foreach (var trustCenter in config.TrustCenters)
            {
                var data = client.DownloadData(trustCenter.TrustCenterURL);
                var str = Encoding.UTF8.GetString(data);
                File.WriteAllText(GetFilePath(trustCenter.Name), str);
            }

            var timeStamp = Convert.ToString(DateTime.Now);
            File.WriteAllText(DataPath + @"Timestamp.JSON", timeStamp);

        }

        private void CreateMissingPath()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
        }

        private string GetFilePath(string name)
        {
            return DataPath + name + @".txt";
        }
    }
}