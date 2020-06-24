using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class SearchManager
    {
        public ObservableCollection<SearchResultsAndBorder> SearchManagerConnector(string search, List<Certificate> certificates)
        {
            if (certificates == null) return null;

            var searchResults = new ObservableCollection<SearchResultsAndBorder>();

            if (string.IsNullOrWhiteSpace(search))
            {
                foreach (var c in certificates)
                    searchResults.Add(new SearchResultsAndBorder {SearchCertificate = c});

                return searchResults;
            }

            foreach (var c in certificates)
            {
                var searchAndCertifcateContentTheSame = new SearchResultsAndBorder();
                var isASearchResult = false;

                if (c.Issuer.Contains(search))
                {
                    searchAndCertifcateContentTheSame.IssuerBorder = "Red";
                    isASearchResult = true;
                }
                if (c.Subject.Contains(search))
                {
                    searchAndCertifcateContentTheSame.SubjectBorder = "Red";
                    isASearchResult = true;
                }
                if (c.SerialNumber.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.SerialNumberBorder = "Red";
                    isASearchResult = true;
                }
                if (c.NotBefore.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotBeforeBorder = "Red";
                    isASearchResult = true;
                }
                if (c.NotAfter.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotAfterBorder = "Red";
                    isASearchResult = true;
                }
                if (c.Thumbprint.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.ThumbprintBorder = "Red";
                    isASearchResult = true;
                }

                if (isASearchResult)
                {
                    searchAndCertifcateContentTheSame.SearchCertificate = c;
                    searchResults.Add(searchAndCertifcateContentTheSame);
                }
            }

            return searchResults;
        }
    }
}