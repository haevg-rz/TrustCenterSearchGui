using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrustCenterSearch.Core.DataManagement
{
    public class DownloadManager
    {
        internal async Task DownloadTrustCenter(string trustCenterName, string trustCenterUrl, string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var client = new HttpClient();

            var response = await client.GetAsync(trustCenterUrl);

            var content = response.Content.ReadAsStringAsync();

            //var str = Encoding.UTF8.GetString(content);

            File.WriteAllText(GetFilePath(trustCenterName, filePath), content.Result);
        }

        internal string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }

        internal bool IsUrlExisting(string url)
        {
            if (url == string.Empty)
                return false;

            var trustCenetrUrl = "https://trustcenter-data.itsg.de/";
            if (!url.Contains(trustCenetrUrl))
                return false;

            try
            {
                var urlCheck = new Uri(url);
                var request = WebRequest.Create(urlCheck);
                request.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}