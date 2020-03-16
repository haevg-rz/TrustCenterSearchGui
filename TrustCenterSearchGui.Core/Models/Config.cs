using System.Collections.Generic;

namespace TrustCenterSearchGui.Core
{
    public class Config
    {
        public List<Webpage> Webpages { get; set; }
    }

    public class Webpage
    {
        public string Link { get; set; }
    }
}