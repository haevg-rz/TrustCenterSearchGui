

namespace TrustCenterSearchGui.Core.Models
{
    public class SearchResultsAndBorder
    {
        public int SubjectBorder { get; set; }
        public int IssuerBorder { get; set; }
        public int SerialNumberBorder { get; set; }
        public int NotBeforeBorder { get; set; }
        public int NotAfterBorder { get; set; }
        public int ThumbprintBorder { get; set; }
        public Certificate SearchCertificate { get; set; }

    }
}
