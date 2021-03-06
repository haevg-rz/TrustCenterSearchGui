﻿using System;
using System.Linq;
using Moq;
using TestSamples;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearch.Core.Test
{
    public class CoreTest
    {
        [Fact(DisplayName = "ImportAllCertificatesFromTrustCentersAsyncTest")]
        public async void ImportAllCertificatesFromTrustCentersAsyncTest()
        {
            #region Arrange

            var moqTrustCenterManager = new Mock<TrustCenterManager> {CallBase = true};
            moqTrustCenterManager.Setup(m => m.ImportCertificatesAsync(It.IsAny<TrustCenterMetaInfo>()))
                .Returns(Samples.ProvideTaskIEnumerableCertificate);

            var core = new Core
            {
                TrustCenterManager = moqTrustCenterManager.Object,
                Config = Samples.ProvideSampleConfig()
            };

            #endregion

            #region Act

            var result = await core.ImportAllCertificatesFromTrustCentersAsync();

            #endregion

            #region Assert

            Assert.Equal(9, core.Certificates.Count);

            #endregion
        }

        [Fact(DisplayName = "AddTrustCenterAsyncTest")]
        public void AddTrustCenterAsyncTest()
        {
            #region Arrange

            var moqCore = new Mock<Core>();
            moqCore.Setup(m => m.IsTrustCenterInputValid(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var moqConfigManager = new Mock<ConfigManager> {CallBase = true};
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

            var moqConfigManager = new Mock<ConfigManager> {CallBase = true};
            moqConfigManager.Setup(m => m.SaveConfig(It.IsAny<Config>())).Returns(Samples.ProvideSampleConfig);

            var moqTrustCenterManager = new Mock<TrustCenterManager> {CallBase = true};
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