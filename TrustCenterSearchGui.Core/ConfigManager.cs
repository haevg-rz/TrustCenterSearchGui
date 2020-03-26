using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TrustCenterSearchGui.Core
{
    public class ConfigManager
    {
        private static string ConfigPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.json";

        public Config GetConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                var directoryInfo = new FileInfo(ConfigPath).Directory;
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                var newConfig = new Config()
                {
                    TrustCenters = new List<TrustCenter>
                    {
                        new TrustCenter
                        {
                            Name = String.Empty,
                            TrustCenterURL = String.Empty,
                        }
                    }
                };
                var json = JsonConvert.SerializeObject(newConfig, Formatting.Indented);
                File.WriteAllText(ConfigPath + ".sample", json);
                return new Config();
            }

            // BUG Crash with System.IO.DirectoryNotFoundException: 
            var jsonString = System.IO.File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);

            return config;
        }
    }
}