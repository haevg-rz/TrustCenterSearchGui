using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class SearchManager
    {
        /*public ObservableCollection<SearchResultsAndBorder> MainSearchManager(string search)
        {
            /*var test = new ObservableCollection<Certificate>()
            {
                new Certificate{Issuer = "Test", NotAfter = new DateTime(2,4,12), NotBefore = new DateTime(2020,12,3),SerialNumber = "12", Subject = "Test", Thumbprint = "okea" },
                new Certificate{Issuer = "Tasdfest", NotAfter = new DateTime(2200,4,12), NotBefore = new DateTime(2020,12,3),SerialNumber = "12", Subject = "Testasd", Thumbprint = "oksafea" },
                new Certificate{Issuer = "Tesadfst", NotAfter = new DateTime(2123,4,12), NotBefore = new DateTime(2020,12,3),SerialNumber = "12", Subject = "Tessadt", Thumbprint = "okesadfa" },
                new Certificate{Issuer = "Tsdafest", NotAfter = new DateTime(2213,4,12), NotBefore = new DateTime(2020,12,3),SerialNumber = "12", Subject = "Tesasdt", Thumbprint = "oksadfea" }
            };#1#

            var searchResults = SearchManagerConnector(search, TrustCenterSearchGui.Core.Config);

            return searchResults;
        }*/

        public ObservableCollection<SearchResultsAndBorder> SearchManagerConnector(string search,
            List<Certificate> certificates)
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
                    searchAndCertifcateContentTheSame.IssuerBorder = 1;
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].Subject.Contains(search))
                {
                    searchAndCertifcateContentTheSame.SubjectBorder = 1;
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].SerialNumber.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.SerialNumberBorder = 1;
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].NotBefore.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotBeforeBorder = 1;
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].NotAfter.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.NotAfterBorder = 1;
                    searchAndCertifcateContentTheSame.SearchCertificate = certificates[i];
                    isASearchResult = true;
                }

                if (certificates[i].Thumbprint.ToString().Contains(search))
                {
                    searchAndCertifcateContentTheSame.ThumbprintBorder = 1;
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