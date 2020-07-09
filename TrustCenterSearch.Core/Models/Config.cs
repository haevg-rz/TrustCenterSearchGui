using System.Collections.Generic;

namespace TrustCenterSearch.Core.Models
{
    public class Config
    {
        public List<TrustCenterMetaInfo> TrustCenterMetaInfos { get; set; }

        public Config()
        {
            this.TrustCenterMetaInfos = new List<TrustCenterMetaInfo>();
        }
    }
}