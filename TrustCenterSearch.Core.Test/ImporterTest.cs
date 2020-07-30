using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class ImporterTest
    {
        [Fact(DisplayName = "ReadFileAsyncTest")]
        public void GetCertificatesFromByteArrayTest()
        {
            #region Arrange

            //byte[] byteArray = Samples.ProvidSampleByteArrays();
            //var cer = Samples.ProvideSampleCertificates().Last();
            //var tcInfo = Samples.ProvideSampleMetaInfos().First();

            #endregion


            #region Act

            //var result = Importer.GetCertificatesFromByteArray(tcInfo,byteArray);

            #endregion


            #region Assert

            #endregion

        }

        [Theory]
        [InlineData("", "No Subjectinfo available")]
        [InlineData("CN=Test123, 456","CN=Test123")]
        [InlineData("CN=123, CN=456, OU=789", "CN=123 CN=456 OU=789")]
        [InlineData("CN=123, CN=456, OU=789,123456789", "CN=123 CN=456 OU=789")]
        public void GetSubjectElementsToDisplayTest(string input, string expectedResult)
        {
            #region Arrange

            #endregion


            #region Act

            var result = Importer.GetSubjectElementsToDisplay(input);

            #endregion


            #region Assert

            Assert.Equal(expectedResult, result);

            #endregion

        }
    }
}
