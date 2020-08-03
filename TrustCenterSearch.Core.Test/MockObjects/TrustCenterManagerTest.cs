using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchCore.Test
{
    public class TrustCenterManagerTest : ITrustCenterManager
    {
        public Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        { 
            async Task<IEnumerable<Certificate>> ProvideSampleOfCertificatesAsync() => Samples.ProvideSampleCertificates();

            return ProvideSampleOfCertificatesAsync();
        }

        public void DeleteTrustCenterFile(string trustCenterName)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCertificatesOfTrustCenter(List<Certificate> certificates, string trustCenterName)
        {
            throw new System.NotImplementedException();
        }
    }
}