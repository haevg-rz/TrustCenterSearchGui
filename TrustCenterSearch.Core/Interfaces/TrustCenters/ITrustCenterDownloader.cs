using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces.TrustCenters
{
    internal interface ITrustCenterDownloader
    {
        Task<byte[]> DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath);

        string GetFilePath(string name, string dataFolderPath);
    }
}
