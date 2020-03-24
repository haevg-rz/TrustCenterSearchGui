using System;
using System.Collections.Generic;
using System.Text;

namespace TrustCenterSearchGui.Core.Models
{
    public class Certificate
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string SerialNumber { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime NotAfter { get; set; }
        public string Thumbprint { get; set; }
    }
}
