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
                                                  @"\TrustCenterSearch\Data";

        public void DownloadDataFromConfic(Config confic)
        {
            CreateMissingPath();
            var client = new WebClient();

            var dataList = confic.TrustCenterURLs.Select(link => (client.DownloadData(link))).ToList();

            var timeStamp = Convert.ToString(DateTime.Now);
            File.WriteAllText(DataPath + @"\Timestamp.JSON", timeStamp);

            var jsonString = JsonConvert.SerializeObject(dataList);
            File.WriteAllText(DataPath + @"\Data.JSON", (jsonString));
        }

        private void CreateMissingPath()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
        }
    }
}