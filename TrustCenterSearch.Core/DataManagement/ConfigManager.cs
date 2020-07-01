using System;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement
{
    public class ConfigManager
    {
        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        public Config LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var jsonString = System.IO.File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);
            return config;
        }

        public Config AddTrustCenterToConfig(string name, string url, Config config)
        {
            config.TrustCenters.Add(new TrustCenter(name, url));
            SaveConfig(config);

            return config;
        }

        public bool IsConfigEmpty(Config config)
        {
            return config.TrustCenters.Count == 0;
        }

        private static void SaveConfig(Config config)
        {
            var jsonString = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigPath, jsonString);
        }
    }
}