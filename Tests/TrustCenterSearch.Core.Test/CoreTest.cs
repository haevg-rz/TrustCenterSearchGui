using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSamples;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
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

            var moqTrustCenterManager = new Mock<TrustCenterManager>();
            moqTrustCenterManager.Setup(m => m.ImportCertificatesAsync(It.IsAny<TrustCenterMetaInfo>()))
                .Returns(Samples.ProvideTaskIEnumerableCertificate);
            // moqTrustCenterManager
            //    .Setup(m => m.ImportCertificatesAsyncTest(It.IsAny<List<TrustCenterMetaInfo>>(),
            //       It.IsAny<List<Certificate>>())).Returns(Samples.ProvideTaskIListCertificate());

            var core = new Core
            {
                TrustCenterManager = moqTrustCenterManager.Object,
                Config = Samples.ProvideSampleConfig()
            };

            #endregion


            #region Act

            var a = core.ImportAllCertificatesFromTrustCentersAsync();

            #endregion


            #region Assert

            //Assert.Equal(9, core.Certificates.Count);
            Assert.Equal(9, a.Result.Count);

            #endregion
        }

        [Fact(DisplayName = "AddTrustCenterAsyncTest")]
        public void AddTrustCenterAsyncTest()
        {
            #region Arrange

            var moqCore = new Mock<Core>();
            moqCore.Setup(m => m.IsTrustCenterInputValid(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var moqConfigManager = new Mock<ConfigManager>();
            moqConfigManager.CallBase = true;
            moqConfigManager.Setup(m => m.SaveConfig(It.IsAny<Config>()))
                .Returns(Samples.ProvideSampleConfig);

            var moqTrustCenterManager = new Mock<TrustCenterManager>();
            moqTrustCenterManager.Setup(m => m.DownloadCertificatesAsync(It.IsAny<TrustCenterMetaInfo>()))
                .Returns(Samples.ProvideSampleTaskByteArray);
            moqTrustCenterManager.Setup(m => m.ImportCertificatesAsync(It.IsAny<TrustCenterMetaInfo>()))
                .Returns(Samples.ProvideTaskIEnumerableCertificate);

            var core = moqCore.Object;
            core.ConfigManager = moqConfigManager.Object;
            core.TrustCenterManager = moqTrustCenterManager.Object;

            core.Config.TrustCenterMetaInfos.RemoveAll(m => true);

            #endregion


            #region Act

            var result = core.AddTrustCenterAsync("name", "url");

            #endregion


            #region Assert

            var expectedCount = 1;
            var expectedResult = new TrustCenterMetaInfo("name", "url", DateTime.Now);

            Assert.Equal(expectedCount, core.Config.TrustCenterMetaInfos.Count);
            Assert.Equal(3, core.Certificates.Count);
            Equals(result, expectedResult);

            #endregion
        }

        [Fact(DisplayName = "DeleteTrustCenterTest")]
        public void DeleteTrustCenterTest()
        {
            #region Arrange

            var moqConfigManager = new Mock<ConfigManager>();
            moqConfigManager.CallBase = true;
            moqConfigManager.Setup(m => m.SaveConfig(It.IsAny<Config>())).Returns(Samples.ProvideSampleConfig);

            var moqTrustCenterManager = new Mock<TrustCenterManager>();
            moqTrustCenterManager.CallBase = true;
            moqTrustCenterManager.Setup(m => m.DeleteTrustCenterFile(It.IsAny<string>())).Returns(null);

            var moqCore = new Mock<Core>();
            var core = moqCore.Object;

            core.ConfigManager = moqConfigManager.Object;
            core.TrustCenterManager = moqTrustCenterManager.Object;

            core.Certificates = Samples.ProvideSampleCertificates();
            core.Config = Samples.ProvideSampleConfig();

            #endregion


            #region Act

            core.DeleteTrustCenter(Samples.ProvideSampleMetaInfos().FirstOrDefault());

            #endregion


            #region Assert

            var certificateCount = 1;
            Assert.Equal(2, core.Config.TrustCenterMetaInfos.Count);
            Assert.Equal(certificateCount, core.Certificates.Count);

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