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
            if (certificates == null)
                return null;

            var searchResults = new ObservableCollection<SearchResultsAndBorder>();

            if (String.IsNullOrWhiteSpace(search))
            {
                foreach (var cert in certificates)
                    searchResults.Add(new SearchResultsAndBorder {SearchCertificate = cert});

                return searchResults;
            }

            foreach (var cert in certificates)
            {
                var searchAndCertifcateContentTheSame = new SearchResultsAndBorder();
                var isASearchResult = false;

                if (cert.Issuer.Contains(search))
                {
                    searchAndCertifcateContentTheSame.IssuerBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (cert.Subject.Contains(search))
                {
                    searchAndCertifcateContentTheSame.SubjectBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (cert.SerialNumber.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.SerialNumberBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (cert.NotBefore.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotBeforeBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (cert.NotAfter.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotAfterBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (cert.Thumbprint.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.ThumbprintBorder = "Red";
                    searchAndCertifcateContentTheSame.SearchCertificate = cert;
                    isASearchResult = true;
                }

                if (isASearchResult)
                    searchResults.Add(searchAndCertifcateContentTheSame);
            }

            return searchResults;
        }
    }
}