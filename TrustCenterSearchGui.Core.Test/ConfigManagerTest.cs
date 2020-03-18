using System.Collections.Generic;
using Xunit;
using static Xunit.Assert;

namespace TrustCenterSearchGui.Core.Test
{
    public class ConfigManagerTest
    {
        [Fact(DisplayName = "Get Config from AppData")]
        public void ReadConfig()
        {
            #region Arrange

            var configManager = new TrustCenterSearchGui.Core.ConfigManager();

            var config = new Config();
            config.Webpages = new List<string>()
            {
                new string("link1"),
                new string("link2"),
                new string("link3")
            };

            #endregion

            #region Act

            var result = configManager.GetConfig();

            #endregion

            #region Assert

            Equal(result.Webpages.Count, actual: 3);
            object.Equals(result, config);

            #endregion
        }
    }
}