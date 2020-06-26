using System;
using System.IO;
using Newtonsoft.Json;

namespace TrustCenterSearchGui.Core
{
    public class ConfigManager
    {
        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        public Config GetConfig()
        {
            if (File.Exists(ConfigPath))
            {
                var jsonString = System.IO.File.ReadAllText(ConfigPath);
                var config = JsonConvert.DeserializeObject<Config>(jsonString);
                return config;
            }
            return new Config();
        }

        public bool ConfigIsEmpty(Config config)
        {
            return config.TrustCenters.Count == 0;
        }
    }
}