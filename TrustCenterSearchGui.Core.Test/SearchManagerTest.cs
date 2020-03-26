using System;
using System.Collections.Generic;
using System.Text;
using TrustCenterSearchGui.Core.Models;
using Xunit;

namespace TrustCenterSearchGui.Core.Test
{
    public class SearchManagerTest
    {
        [Theory]
        [InlineData("Test", "Test", true)]
        [InlineData(null, "Test", false)]
        public void SearchManager(string search, string issuer, bool testIssuerBorder)
        {
            #region Arrange

            var searchManager = new SearchManager();
            var certificates = new List<Certificate>()
            {
                new Certificate {Issuer = issuer, NotAfter = new DateTime(2020, 2, 1), NotBefore = new DateTime(2020, 6, 1), SerialNumber = "23", Subject = "Test", Thumbprint = "Test"}
            };

            #endregion


            #region Act

            var searchresult = searchManager.SearchManagerConnector(search, certificates);
            var issuerBorder = searchresult[0].IssuerBorder;

            #endregion


            #region Assert

            object.Equals(issuerBorder, testIssuerBorder);

            #endregion
        }
    }
}