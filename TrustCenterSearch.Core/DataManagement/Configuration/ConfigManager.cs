using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Interfaces.Configuration;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.Configuration
{
    internal class ConfigManager : IConfigManager
    {
        #region Properties

        private static string ConfigPath { get; } =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch\Config.JSON";

        #endregion

        #region Fields

        private readonly string _trustCenterSearchGuiPath =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch";

        private readonly string _configFolderPath =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch\Config.JSON";

        #endregion

        #region ICongigManagerMethods

        public Config LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var jsonString = File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);
            return config;
        }

        public Config AddTrustCenterToConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            config.TrustCenterMetaInfos.Add(trustCenterMetaInfo);
            return config;
        }

        public virtual Config SaveConfig(Config config)
        {
            if (!Directory.Exists(this._trustCenterSearchGuiPath))
                Directory.CreateDirectory(this._trustCenterSearchGuiPath);

            var jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, jsonString);

            return config;
        }

        public Config DeleteTrustCenterFromConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            config.TrustCenterMetaInfos.RemoveAll(tc => tc.Name.Equals(trustCenterMetaInfo.Name));
            return config;
        }

        public bool IsConfigEmpty(Config config)
        {
            return config.TrustCenterMetaInfos.Count == 0;
        }

        public (List<TrustCenterMetaInfo> addedTrustCenterHistoryElements, List<TrustCenterMetaInfo>
            removedTrustCenterHistoryElements, Config config) OpenConfig(Config config)
        {
            if (!File.Exists(this._configFolderPath))
            {
                config = new Config();
                this.SaveConfig(config);
            }

            var psi = new ProcessStartInfo
            {
                FileName = this._configFolderPath,
                UseShellExecute = true,
                Verb = "open"
            };

            var p = Process.Start(psi);
            p.WaitForInputIdle();
            p.WaitForExit();

            if (p.HasExited)
            {
                var newConfig = this.LoadConfig();

                var newElements = newConfig.TrustCenterMetaInfos.Where(newConfigElement =>
                    config.TrustCenterMetaInfos.All(x => x.Name != newConfigElement.Name)).ToList();

                var deletedElements = config.TrustCenterMetaInfos.Where(newConfigElement =>
                    newConfig.TrustCenterMetaInfos.All(x => x.Name != newConfigElement.Name)).ToList();

                config = newConfig;
                return (newElements.ToList(), deletedElements.ToList(), config);
            }

            return (new List<TrustCenterMetaInfo>(), new List<TrustCenterMetaInfo>(), config);
        }
    }

    #endregion
}