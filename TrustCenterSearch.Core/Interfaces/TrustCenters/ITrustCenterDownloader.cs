using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces
{
    internal interface ITrustCenterDownloader
    {
        Task<byte[]> DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath);

        string GetFilePath(string name, string dataFolderPath);
    }
}
