namespace TrustCenterSearch.Core.Models
{
    public class TrustCenterMetaInfo
    {
        public string Name { get; set; }
        public string TrustCenterUrl { get; set; }

        public TrustCenterMetaInfo(string name, string url)
        {
            this.Name = name;
            this.TrustCenterUrl = url;
        }
    }
}