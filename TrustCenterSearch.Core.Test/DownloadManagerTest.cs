using TrustCenterSearch.Core;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class DownloadManagerTest
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

            var result = core.TrustCenterManager.DownloadManager.IsUrlExisting(url);

            #endregion


            #region Assert

            Assert.Equal(result, aspectedResult);

            #endregion

        }
    }
}
