using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces
{
    internal interface ITrustCenterImporter
    {
        Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath);
    }
}
