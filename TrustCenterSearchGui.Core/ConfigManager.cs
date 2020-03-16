using System;
using System.Text;
using Newtonsoft.Json;

namespace TrustCenterSearchGui.Core
{
    internal class ConfigManager
    {
        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        public Config GetConfic()
        {
            try
            {
                var jsonString = System.IO.File.ReadAllText(ConfigPath);

                var config = JsonConvert.DeserializeObject<Config>(jsonString);

                if (config != null)
                    return config;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
    }
}