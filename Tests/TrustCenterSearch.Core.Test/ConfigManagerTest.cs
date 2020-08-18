using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestSamples;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearch.Core.Test
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
                trustCenterMetaInfo => trustCenterMetaInfo.Name.Equals(metaDataToDelete.Name));

            #endregion
        }

        [Fact(DisplayName = "GetRelativeTrustCenterMetaInfoComplementTest")]
        public void GetRelativeTrustCenterMetaInfoComplementTest()
        {
            #region Arrange

            var config = Samples.ProvideSampleConfig();
            var newConfig = Samples.ProvideSampleConfig();
            newConfig.TrustCenterMetaInfos = Samples.ProvideSampleMetaInfos().TakeLast<TrustCenterMetaInfo>(2).ToList();

            var expectedResult = new List<TrustCenterMetaInfo>() { Samples.ProvideSampleMetaInfos()[0] };

            #endregion

            #region Act

            var result = ConfigManager.GetRelativeTrustCenterMetaInfoComplement(config, newConfig);

            #endregion

            # region Assert

            expectedResult.Should().BeEquivalentTo(result);
      
            #endregion
        }
    }
}