using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces.TrustCenters
{
    internal interface ITrustCenterManager
    {
        Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo);

        bool DeleteTrustCenterFile(string trustCenterName);

        List<Certificate> DeleteCertificatesOfTrustCenter(List<Certificate> certificates, string trustCenterName);
    }
}
