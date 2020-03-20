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
            config.TrustCenters = new List<TrustCenter>()
            {
                new TrustCenter(){Name = "Example1", TrustCenterURL = "link1"},
                new TrustCenter(){Name = "Example2", TrustCenterURL = "link2"},
                new TrustCenter(){Name = "Example3", TrustCenterURL = "link3"}
            };

            #endregion

            #region Act

            var result = configManager.GetConfig();

            #endregion

            #region Assert

            Equal(result.TrustCenters.Count, actual: 3);
            object.Equals(result, config);

            #endregion
        }
    }
}