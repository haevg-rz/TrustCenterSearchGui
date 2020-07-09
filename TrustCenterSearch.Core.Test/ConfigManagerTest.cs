using Xunit;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.Models;

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

            core.ConfigManager.AddTrustCenterToConfig(new TrustCenterMetaInfo("test123", "test567"), config);

            #endregion


            #region Assert

            var result = new Config();
            result.TrustCenterMetaInfos.Add(new TrustCenterMetaInfo("test123", "test567"));

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

            Assert.Equal(result, true);

            #endregion

        }

        [Fact(DisplayName = "ConfigIsNotEmptyTest")]
        public void ConfigIsNotEmptyTest()
        {
            #region Arrange

            var config = new Config();
            config.TrustCenterMetaInfos.Add(new TrustCenterMetaInfo("test", "test"));

            var core = new Core();

            #endregion


            #region Act

            var result = core.ConfigManager.IsConfigEmpty(config);

            #endregion


            #region Assert

            Assert.Equal(result, false);

            #endregion

        }
    }
}

