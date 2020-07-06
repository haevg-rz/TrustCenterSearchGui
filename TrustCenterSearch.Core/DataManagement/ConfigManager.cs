using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement
{
    public class ConfigManager
    {
        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        internal Config LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var jsonString = File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);
            return config;
        }

        internal TrustCenter AddTrustCenterToConfig(string name, string url, Config config)
        {
            var trustCenter = new TrustCenter(name, url);
            config.TrustCenters.Add(trustCenter);
            return trustCenter;
        }

        internal Config SaveConfig(Config config)
        {
            var jsonString = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigPath, jsonString);
            return config;
        }

        internal bool IsConfigEmpty(Config config)
        {
            return config.TrustCenters.Count == 0;
        }
    }
}