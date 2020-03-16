using System;
using System.Text;
using Newtonsoft.Json;

namespace TrustCenterSearchGui.Core
{
    class ConfigManager
    {
        private static string ConfigPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";
        public Config GetConfic()
        {
            var jsonString = System.IO.File.ReadAllText(ConfigPath);

            var config = JsonConvert.DeserializeObject<Config>(jsonString);

            return config ?? null;
        }
    }
}
