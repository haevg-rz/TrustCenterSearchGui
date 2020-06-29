using System.Collections.Generic;

namespace TrustCenterSearch.Core.Models
{
    public class Config
    {
        public List<TrustCenter> TrustCenters { get; set; }

        public Config()
        {
            this.TrustCenters = new List<TrustCenter>();
        }
    }

    public class TrustCenter
    {
        public string Name { get; set; }
        public string TrustCenterURL { get; set; }

        public TrustCenter(string name, string url)
        {
            this.Name = name;
            this.TrustCenterURL = url;
        }

        public TrustCenter()
        {
            this.Name = string.Empty;
            this.TrustCenterURL = string.Empty;
        }
    }
}