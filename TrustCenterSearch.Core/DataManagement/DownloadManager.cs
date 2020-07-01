using System;
using System.IO;
using System.Net;
using System.Text;

namespace TrustCenterSearch.Core.DataManagement
{
    public class DownloadManager
    {
        internal void DownloadTrustCenter(string trustCenterName, string trustCenterUrl, string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var client = new WebClient();

            var data = client.DownloadData(trustCenterUrl);
            var str = Encoding.UTF8.GetString(data);
            File.WriteAllText(GetFilePath(trustCenterName, filePath), str);
        }

        internal string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }

        internal bool IsUrlExisting(string url)
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