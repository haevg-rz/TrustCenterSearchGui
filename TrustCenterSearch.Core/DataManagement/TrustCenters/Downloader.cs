using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

[assembly: InternalsVisibleTo("TrustCenterSearchCore.Test")]

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    public class Downloader:ITrustCenterDownloader
    {
        #region ITrustCenterDownloaderMethods

        public async Task<byte[]> DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath)
        {
            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            var client = new HttpClient();

            var response = await client.GetAsync(trustCenterMetaInfo.TrustCenterUrl);

            var content = await response.Content.ReadAsByteArrayAsync();

            using (var stream = File.OpenWrite(GetFilePath(trustCenterMetaInfo.Name, dataFolderPath)))
            {
                await stream.WriteAsync(content, 0, content.Length - 1);
            }

            return content;
        }

        public string GetFilePath(string name, string dataFolderPath)
        {
            return $@"{dataFolderPath}{name}.txt";
        }

        #endregion

        #region InternalStaticMethods
        internal static bool IsUrlExisting(string url)
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