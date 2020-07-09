using System.Collections.Generic;

namespace TrustCenterSearch.Core.Models
{
    public class TrustCenter
    {
        public TrustCenterMetaInfo TrustCenterMetaInfo { get; set; }
        public List<Certificate> Certificates { get; set; }
        public TrustCenter(TrustCenterMetaInfo trustCenterMetaInfo, List<Certificate> certificates)
        {
            this.TrustCenterMetaInfo = trustCenterMetaInfo;
            this.Certificates = certificates;
        }
    }
}