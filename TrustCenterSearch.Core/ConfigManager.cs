using System;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core
{
    public class ConfigManager
    {
        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        public Config GetConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var jsonString = System.IO.File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);
            return config;
        }

        public Config AddTrustCenterToConfig(string name, string url, Config config)
        {
            config = this.AddToConfig(name, url, config);  //TODO: review naming
            this.SaveConfig(config);

            return config;
        }

        private Config AddToConfig(string name, string url, Config config)
        {
           config.TrustCenters.Add(new TrustCenter(name, url));

           return config;
        }

        private void SaveConfig(Config config)
        {
            var jsonString = JsonConvert.SerializeObject(config);
            System.IO.File.WriteAllText(ConfigPath, jsonString);
        }

        public bool ConfigIsEmpty(Config config)
        {
            return config.TrustCenters.Count == 0;
        }
    }
}