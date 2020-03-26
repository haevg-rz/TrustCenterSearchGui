using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
                for (var i = 0; i < certificates.Count; i++)
                    searchResults.Add(new SearchResultsAndBorder {SearchCertificate = certificates[i]});

                return searchResults;
            }

            for (var i = 0; i < certificates.Count; i++)
            {
                var searchAndCertifcateContentTheSame = new SearchResultsAndBorder();
                var isASearchResult = false;

                if (certificates[i].Issuer.Contains(search))
                {
                    searchAndCertifcateContentTheSame.IssuerBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }
                if (certificates[i].Subject.Contains(search))
                {
                    searchAndCertifcateContentTheSame.SubjectBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }
                if (certificates[i].SerialNumber.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.SerialNumberBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }
                if (certificates[i].NotBefore.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotBeforeBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].NotAfter.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotAfterBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].Thumbprint.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.ThumbprintBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (isASearchResult)
                    searchResults.Add(searchAndCertifcateContentTheSame);
            }

            return searchResults;
        }
    }
}