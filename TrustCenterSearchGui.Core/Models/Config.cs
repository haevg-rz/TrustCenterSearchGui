using System.Collections;
using System.Collections.Generic;

namespace TrustCenterSearchGui.Core
{
    public class Config
    {
        public List<TrustCenter> TrustCenters { get; set; }
    }

    public class TrustCenter
    {
        public string Name { get; set; }
        public string TrustCenterURL { get; set; }
    }
}