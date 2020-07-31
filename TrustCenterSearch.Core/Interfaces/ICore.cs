using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.Interfaces
{
    public interface ICore
    {
        Task<List<Certificate>> ImportAllCertificatesFromTrustCentersAsync();
        Task<TrustCenterMetaInfo> AddTrustCenterAsync(string newTrustCenterName, string newTrustCenterUrl);

        void DeleteTrustCenter(TrustCenterMetaInfo trustCenterMetaInfo);

        Task<TrustCenterMetaInfo> ReloadCertificatesOfTrustCenter(TrustCenterMetaInfo trustCenterMetaInfo);

        List<TrustCenterMetaInfo> GetTrustCenterHistory();

        List<Certificate> GetCertificates();
    }
}
