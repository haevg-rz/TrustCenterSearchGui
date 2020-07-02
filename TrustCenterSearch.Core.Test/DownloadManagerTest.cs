using TrustCenterSearch.Core;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class DownloadManagerTest
    {
        [Theory]
        [InlineData("google.com/", false)]
        [InlineData("https://trustcenter-data.itsg.de/", true)]
        [InlineData("test123", false)]
        public void IsUrlExistingTest(string url, bool aspectedResult)
        {
            #region Arrange

            var core = new Core();

            #endregion


            #region Act

            var result = core.DownloadManager.IsUrlExisting(url);

            #endregion


            #region Assert

            Assert.Equal(result, aspectedResult);

            #endregion

        }
    }
}
