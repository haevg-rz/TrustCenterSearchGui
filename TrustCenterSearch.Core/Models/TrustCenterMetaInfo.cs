using System;

namespace TrustCenterSearch.Core.Models
{
    public class TrustCenterMetaInfo
    {

        public string Name { get; set; }
        public string TrustCenterUrl { get; set; }
        public DateTime LastUpdate { get; set; }

        public TrustCenterMetaInfo(string name, string url, DateTime lastUpdate)
        {
            this.Name = name;
            this.TrustCenterUrl = url;
            this.LastUpdate = lastUpdate;
        }

    }
}