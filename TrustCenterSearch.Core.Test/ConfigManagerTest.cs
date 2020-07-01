using System;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.DataManagement;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class ConfigManagerTest
    {
        private static string ConfigTestPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\test";

        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        [Fact(DisplayName = "Get Config from AppData")]
        public void ReadConfig()
        {
            #region Arrange

            var configManager = new ConfigManager();

            #endregion

            #region Act

            var result = configManager.LoadConfig();

            #endregion

            #region Assert

            if (!Directory.Exists(ConfigTestPath))
                Directory.CreateDirectory(ConfigTestPath);

            var jsonString = JsonConvert.SerializeObject(result, Formatting.Indented);
            System.IO.File.WriteAllText(ConfigTestPath+ @"\ConfigTest.JSON", jsonString);

            object.Equals(ConfigPath, ConfigTestPath);

            #endregion
        }
    }
}