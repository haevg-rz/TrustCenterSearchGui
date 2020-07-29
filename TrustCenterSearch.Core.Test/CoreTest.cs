
using System.ComponentModel;
using TrustCenterSearch.Core;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class CoreTest
    {
        [Fact(DisplayName = "ImportAllCertificatesFromTrustCentersAsyncTest")]
        public void ImportAllCertificatesFromTrustCentersAsyncTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

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
        [InlineData("TooLongTrustCenterNameForTesting","TestURL", "One or more errors occurred. (The entered name is too long.)")]
        [InlineData("","TestURL", "One or more errors occurred. (The entered name must not be empty.)")]
        [InlineData("TestName","TestURL", "One or more errors occurred. (The entered Url is not valid.)")]
        public void IsTrustCenterInputValidTest(string newTrustCenterNameTest,string newTrustCenterUrlTest,string errorMessage)
        {
            #region Arrange

            #endregion


            #region Act

            var core = new Core();
            var exceptionResult = core.AddTrustCenterAsync(newTrustCenterNameTest, newTrustCenterUrlTest);

            #endregion


            #region Assert

            Assert.Equal(errorMessage,exceptionResult.Exception.Message);

            #endregion

        }
    }
}
