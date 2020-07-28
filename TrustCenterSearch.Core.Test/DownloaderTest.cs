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

            #endregion


            #region Act

            var result = Downloader.IsUrlExisting(url);

            #endregion


            #region Assert

            Assert.Equal(result, aspectedResult);

            #endregion

        }

        [Fact(DisplayName = "DownloadCertificatesTest")]
        public void DownloadCertificatesTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Theory]
        [InlineData("test123", "test", "testtest123.txt")]
        [InlineData("123", "", "123.txt")]
        [InlineData("", "test", "test.txt")]
        public void GetFilePathTest(string name, string dataFolderPath, string aspected)
        {
            #region Arrange

            var downloader = new Downloader();

            #endregion

            #region Act

            var result = downloader.GetFilePath(name, dataFolderPath);

            #endregion

            #region Assert

            Assert.Equal(result, aspected);

            #endregion

        }
    }
}
