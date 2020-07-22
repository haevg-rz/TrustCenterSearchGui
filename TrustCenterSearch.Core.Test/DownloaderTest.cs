using TrustCenterSearch.Core;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class DownloaderTest
    {
        [Theory]
        [InlineData("google.com/", false)]
        [InlineData("test123", false)]
        public void IsUrlExistingTest(string url, bool aspectedResult)
        {
            #region Arrange

            var core = new Core();

            #endregion


            #region Act

            var result = Downloader.IsUrlExisting(url);

            #endregion


            #region Assert

            Assert.Equal(result, aspectedResult);

            #endregion

        }
    }
}
