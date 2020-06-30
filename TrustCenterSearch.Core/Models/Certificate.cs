using System;

namespace TrustCenterSearch.Core.Models
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
