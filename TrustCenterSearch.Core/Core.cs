﻿using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using TrustCenterSearch.Core.Interfaces;
using TrustCenterSearch.Core.Interfaces.Configuration;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

[assembly: InternalsVisibleTo("TrustCenterSearch.Core.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TrustCenterSearch.Core
{
    public class Core : ICore
    {
        #region Properties

        internal ITrustCenterManager TrustCenterManager { get; set; } = new TrustCenterManager();
        internal IConfigManager ConfigManager { get; set; } = new ConfigManager();
        internal Config Config { get; set; }
        internal List<Certificate> Certificates { get; set; } = new List<Certificate>();

        #endregion

        #region Constructor

        public Core()
        {
            this.Config = this.ConfigManager.LoadConfig();
        }

        #endregion

        #region PublicMethods

        public async Task<List<Certificate>> ImportAllCertificatesFromTrustCentersAsync()
        {
            return await this.TrustCenterManager.ImportCertificatesAsync(this.Config.TrustCenterMetaInfos,
                this.Certificates);
        }

        public async Task<TrustCenterMetaInfo> AddTrustCenterAsync(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (!this.IsTrustCenterInputValid(newTrustCenterName, newTrustCenterUrl))
                return null;

            var newTrustCenterMetaInfo = new TrustCenterMetaInfo(newTrustCenterName, newTrustCenterUrl, DateTime.Now);
            await this.TrustCenterManager.DownloadCertificatesAsync(newTrustCenterMetaInfo);
            var importedCertificates = await this.TrustCenterManager.ImportCertificatesAsync(newTrustCenterMetaInfo);
            this.Certificates.AddRange(importedCertificates);
            this.ConfigManager.AddTrustCenterToConfig(newTrustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);

            return newTrustCenterMetaInfo;
        }

        public void DeleteTrustCenter(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.ConfigManager.DeleteTrustCenterFromConfig(trustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            this.TrustCenterManager.DeleteTrustCenterFile(trustCenterMetaInfo.Name);
            this.TrustCenterManager.DeleteCertificatesOfTrustCenter(this.Certificates, trustCenterMetaInfo.Name);
        }

        public async Task<TrustCenterMetaInfo> ReloadCertificatesOfTrustCenter(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.DeleteTrustCenter(trustCenterMetaInfo);
            return await this.AddTrustCenterAsync(trustCenterMetaInfo.Name, trustCenterMetaInfo.TrustCenterUrl);
        }

        public virtual List<TrustCenterMetaInfo> GetTrustCenterHistory()
        {
            return this.Config.TrustCenterMetaInfos;
        }

        public List<Certificate> GetCertificates()
        {
            return this.Certificates;
        }

        public async Task<List<Certificate>> OpenConfig()
        {
            (List<TrustCenterMetaInfo>addedElements, List<TrustCenterMetaInfo>deletedElements, Config config)
                changesInConfig = this.ConfigManager.OpenConfig(this.Config);
            this.Config = changesInConfig.config;

            await this.TrustCenterManager.ImportCertificatesAsync(changesInConfig.addedElements, this.Certificates);

            foreach (var deletedElement in changesInConfig.deletedElements)
                this.TrustCenterManager.DeleteCertificatesOfTrustCenter(this.Certificates, deletedElement.Name);

            return this.Certificates;
        }

        #endregion

        #region InternalMethods

        internal virtual bool IsTrustCenterInputValid(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (newTrustCenterName.Length > 24)
                throw new ArgumentException("The entered name is too long.");

            if (newTrustCenterName == string.Empty)
                throw new ArgumentException("The entered name must not be empty.");

            if (newTrustCenterName.Intersect(new char[]
                {'~', '#', '%', '&', '*', ':', '<', '>', '?', '/', '{', '|', '}'}).Any())
                throw new ArgumentException("Invalid file characters are: ~ #% & *: <>? /  {|}.");

            if (!Downloader.IsUrlExisting(newTrustCenterUrl))
                throw new ArgumentException("Can not access Url.");

            if (this.Config.TrustCenterMetaInfos.Any(tc => tc.Name.Equals(newTrustCenterName)))
                throw new ArgumentException("The entered name is already added.");

            if (this.Config.TrustCenterMetaInfos.Any(tc => tc.TrustCenterUrl.Equals(newTrustCenterUrl)))
                throw new ArgumentException("The entered Url is already added.");

            return true;
        }

        #endregion
    }
}