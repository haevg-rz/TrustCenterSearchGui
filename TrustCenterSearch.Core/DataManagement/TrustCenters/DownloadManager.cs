using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class DownloadManager
    {
        #region InternalMethods

        internal async Task<byte[]> DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath)
        {
            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            var client = new HttpClient();

            var response = await client.GetAsync(trustCenterMetaInfo.TrustCenterUrl);

            var content = response.Content.ReadAsByteArrayAsync();

            using (var stream = File.OpenWrite(GetFilePath(trustCenterMetaInfo.Name, dataFolderPath)))
            {
                await stream.WriteAsync(content.Result, 0, content.Result.Length - 1);
            }

            return content.Result;
        }

        internal string GetFilePath(string name, string dataFolderPath)
        {
            return $@"{dataFolderPath}{name}.txt";
        }

        internal bool IsUrlExisting(string url)
        {
            if (url == String.Empty)
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

        #endregion
    }
}