using System;

namespace TrustCenterSearch.Core.Models
{
    public class Certificate
    {
        public string Subject { get; set; }
        public string SerialNumber { get; set; }
        public string NotBefore { get; set; }
        public string NotAfter { get; set; }
        public string Thumbprint { get; set; }
        public string TrustCenterName { get; set; }
        public string PublicKeyLength { get; set; }
    }
}
