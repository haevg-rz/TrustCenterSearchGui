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
        public void DownloadDataFromConfic(Config config, string filePath)
        {
            var dataManager = new DataManager();
            dataManager.CreateMissingPath(filePath);

            var client = new WebClient();

            foreach (var trustCenter in config.TrustCenters)
            {
                var data = client.DownloadData(trustCenter.TrustCenterURL);
                var str = Encoding.UTF8.GetString(data);
                File.WriteAllText(GetFilePath(trustCenter.Name, filePath), str);
            }
        }

        private string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }
    }
}