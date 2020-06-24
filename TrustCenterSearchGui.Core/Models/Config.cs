using System.Collections;
using System.Collections.Generic;

namespace TrustCenterSearchGui.Core
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
    }
}