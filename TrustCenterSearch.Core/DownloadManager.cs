using System;
using System.IO;
using System.Net;
using System.Text;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core
{
    public class DownloadManager
    {
        public void DownloadDataFromConfig(Config config, string filePath)
        {
            var dataManager = new DataManager();
            dataManager.CreateDirectoryIfMissing(filePath);

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

        public bool IsUrlExisting(string url)
        {
            if (url == string.Empty)
                return false;
            try
            {
                var urlCheck = new Uri(url);
                var request = WebRequest.Create(urlCheck);
                var response = request.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}