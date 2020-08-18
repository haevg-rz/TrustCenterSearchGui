using TrustCenterSearch.Core.DataManagement.TrustCenters;
using Xunit;

namespace TrustCenterSearch.Core.Test
{
    public class DownloaderTest
    {
        [Theory]
        [InlineData("google.com/", false)]
        [InlineData("", false)]
        [InlineData("test123", false)]
        public void IsUrlExistingTest(string url, bool expectedResult)
        {
            #region Arrange

            #endregion

            #region Act

            var result = Downloader.IsUrlExisting(url);

            #endregion

            #region Assert

            Assert.Equal(result, expectedResult);

            #endregion
        }

        [Theory]
        [InlineData("test123", "test", "testtest123.txt")]
        [InlineData("123", "", "123.txt")]
        [InlineData("", "test", "test.txt")]
        public void GetFilePathTest(string name, string dataFolderPath, string expected)
        {
            #region Arrange

            var downloader = new Downloader();

            #endregion

            #region Act

            var result = downloader.GetFilePath(name, dataFolderPath);

            #endregion

            #region Assert

            Assert.Equal(result, expected);

            #endregion
        }
    }
}