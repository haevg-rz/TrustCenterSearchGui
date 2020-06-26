

namespace TrustCenterSearch.Core.Models
{
    public class SearchResultsAndBorder
    {
        public string SubjectBorder { get; set; } = "Black";
        public string IssuerBorder { get; set; } = "Black";
        public string SerialNumberBorder { get; set; } = "Black";
        public string NotBeforeBorder { get; set; } = "Black";
        public string NotAfterBorder { get; set; } = "Black";
        public string ThumbprintBorder { get; set; } = "Black";
        public Certificate SearchCertificate { get; set; } 

    }
}
