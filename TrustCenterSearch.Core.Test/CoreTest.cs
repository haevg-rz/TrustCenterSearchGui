using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class CoreTest
    {
        [Fact(DisplayName = "ImportAllCertificatesFromTrustCentersAsyncTest")]
        public void ImportAllCertificatesFromTrustCentersAsyncTest()
        {
            #region Arrange

            var core = new Core();

            core.TrustCenterManager = new TrustCenterManagerTest();
            core.Config = Samples.ProvideSampleConfig();

            #endregion


            #region Act

            var result = core.ImportAllCertificatesFromTrustCentersAsync();

            #endregion


            #region Assert

            Assert.Equal(9, core.Certificates.Count);

            #endregion
        }

        [Fact(DisplayName = "AddTrustCenterAsyncTest")]
        public void AddTrustCenterAsyncTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion
        }

        [Fact(DisplayName = "ReloadCertificatesOfTrustCenterTest")]
        public void ReloadCertificatesOfTrustCenterTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion
        }

        [Theory]
        [InlineData("TooLongTrustCenterNameForTesting", "TestURL",
            "One or more errors occurred. (The entered name is too long.)")]
        [InlineData("", "TestURL", "One or more errors occurred. (The entered name must not be empty.)")]
        [InlineData("TestName", "TestURL", "One or more errors occurred. (Can not access Url.)")]
        public void IsTrustCenterInputValidTest(string newTrustCenterNameTest, string newTrustCenterUrlTest,
            string errorMessage)
        {
            #region Arrange

            #endregion


            #region Act

            var core = new Core();
            var exceptionResult = core.AddTrustCenterAsync(newTrustCenterNameTest, newTrustCenterUrlTest);

            #endregion


            #region Assert

            Assert.Equal(errorMessage, exceptionResult.Exception.Message);

            #endregion
        }
    }
}