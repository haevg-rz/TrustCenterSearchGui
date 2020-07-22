using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces
{
    internal interface ITrustCenterManager
    {
        Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        void DeleteTrustCenterFile(string trustCenterName);

        void DeleteCertificatesOfTrustCenter(IEnumerable<Certificate> certificates, string trustCenterName);
    }
}
