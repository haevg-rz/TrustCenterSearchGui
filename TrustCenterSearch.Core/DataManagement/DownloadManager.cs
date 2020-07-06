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
        private string TrustCenterUrl { get; } = "https://trustcenter-data.itsg.de/";
        internal async Task DownloadTrustCenter(string trustCenterName, string trustCenterUrl, string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var client = new HttpClient();

            var response = await client.GetAsync(trustCenterUrl);

            var content = response.Content.ReadAsByteArrayAsync();

            using (var stream = File.OpenWrite(GetFilePath(trustCenterName, filePath)))
            {
                await stream.WriteAsync(content.Result, 0, content.Result.Length-1);
            }
        }

        internal string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }

        internal bool IsUrlExisting(string url)
        {
            if (url == string.Empty)
                return false;

            if (!url.Contains(this.TrustCenterUrl))
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