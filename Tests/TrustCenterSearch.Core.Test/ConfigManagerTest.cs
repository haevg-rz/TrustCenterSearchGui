using System;
using System.Linq;
using Xunit;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.Models;
using TestSamples;

namespace TrustCenterSearchCore.Test
{
    public class ConfigManagerTest
    {
        [Fact(DisplayName = "AddToConfigTest")]
        public void AddTrustCenterToConfigTest()
        {
            #region Arrange

            var config = new Config();
            var core = new Core();

            #endregion


            #region Act

            core.ConfigManager.AddTrustCenterToConfig(new TrustCenterMetaInfo("test123", "test567", DateTime.Now),
                config);

            #endregion


            #region Assert

            var result = new Config();
            result.TrustCenterMetaInfos.Add(new TrustCenterMetaInfo("test123", "test567", DateTime.Now));

            object.Equals(result, config);

            #endregion
        }

        [Fact(DisplayName = "ConfigIsEmptyTest")]
        public void ConfigIsEmptyTest()
        {
            #region Arrange

            var config = new Config();
            var core = new Core();

            #endregion


            #region Act

            var result = core.ConfigManager.IsConfigEmpty(config);

            #endregion


            #region Assert

            Assert.True(result);

            #endregion
        }

        [Fact(DisplayName = "ConfigIsNotEmptyTest")]
        public void ConfigIsNotEmptyTest()
        {
            #region Arrange

            var config = new Config();
            config.TrustCenterMetaInfos.Add(new TrustCenterMetaInfo("test", "test", DateTime.Now));

            var core = new Core();

            #endregion


            #region Act

            var result = core.ConfigManager.IsConfigEmpty(config);

            #endregion


            #region Assert
            
            Assert.False(result);

            #endregion
        }

        [Fact(DisplayName = "DeleteTrustCenterFromConfigTest")]
        public void DeleteCertificatesOfTrustCenterTest()
        {
            #region Arrange

            var configSample = Samples.ProvideSampleConfig();

            var trustCenterManager = new ConfigManager();

            var metaDataToDelete = configSample.TrustCenterMetaInfos.FirstOrDefault();

            #endregion


            #region Act

            trustCenterManager.DeleteTrustCenterFromConfig(metaDataToDelete, configSample);

            #endregion


            #region Assert

            Assert.DoesNotContain(configSample.TrustCenterMetaInfos,
                trustCentermetaInfo => trustCentermetaInfo.Name.Equals(metaDataToDelete.Name));

            #endregion
        }
    }
}