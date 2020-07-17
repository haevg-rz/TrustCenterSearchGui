﻿using System;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.Configuration
{
    internal class ConfigManager
    {
        #region Properties

        private static string ConfigPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\Config.JSON";

        #endregion

        #region Fields

        private readonly string _trustCenterSearchGuiPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch";

        #endregion

        #region InternalMethods
        internal Config LoadConfig()
        {
            if (!File.Exists(ConfigPath))
                return new Config();

            var jsonString = File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<Config>(jsonString);
            return config;
        }

        internal void AddTrustCenterToConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            config.TrustCenterMetaInfos.Add(trustCenterMetaInfo);
        }

        internal void SaveConfig(Config config)
        {
            if (!Directory.Exists(_trustCenterSearchGuiPath))
                Directory.CreateDirectory(_trustCenterSearchGuiPath);

            var jsonString = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigPath, jsonString);
        }

        internal void DeleteTrustCenterFromConfig(TrustCenterMetaInfo trustCenterMetaInfo, Config config)
        {
            config.TrustCenterMetaInfos.RemoveAll(tc => tc.Name.Equals(trustCenterMetaInfo.Name));
        }

        internal bool IsConfigEmpty(Config config)
        {
            return config.TrustCenterMetaInfos.Count == 0;
        }

        #endregion
    }
}