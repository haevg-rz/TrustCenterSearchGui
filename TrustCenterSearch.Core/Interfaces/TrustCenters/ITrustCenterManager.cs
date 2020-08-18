using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces.TrustCenters
{
    internal interface ITrustCenterManager
    {
        Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        Task<List<Certificate>> ImportCertificatesAsync(List<TrustCenterMetaInfo> trustCenterMetaInfos,
            List<Certificate> certificates);

        bool DeleteTrustCenterFile(string trustCenterName);

        List<Certificate> DeleteCertificatesOfTrustCenter(List<Certificate> certificates, string trustCenterName);
    }
}
