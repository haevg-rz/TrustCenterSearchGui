﻿using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using TrustCenterSearch.Core.Models;

[assembly: InternalsVisibleTo("TrustCenterSearchCore.Test")]

namespace TrustCenterSearch.Core
{
    public class Core
    {
        #region Properties
        internal TrustCenterManager TrustCenterManager { get; set; } = new TrustCenterManager();
        internal ConfigManager ConfigManager { get; set; } = new ConfigManager();
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
        public async Task ImportAllCertificatesFromTrustCenters()
        {
            var importTasks = Config.TrustCenterMetaInfos.Select(trustCenterMetaInfo => this.TrustCenterManager.ImportCertificates(trustCenterMetaInfo)).ToList();
            await Task.WhenAll(importTasks);

            foreach (var importTask in importTasks)
            {
                Certificates.AddRange(importTask.Result);
            }
        }

        public async Task AddTrustCenter(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (!this.IsTrustCenterInputValid(newTrustCenterName, newTrustCenterUrl))
                return;

            var newTrustCenterMetaInfo = new TrustCenterMetaInfo(newTrustCenterName, newTrustCenterUrl);
            this.ConfigManager.AddTrustCenterToConfig(newTrustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            await this.TrustCenterManager.DownloadCertificates(newTrustCenterMetaInfo);
            var importedCertificates =  await this.TrustCenterManager.ImportCertificates(newTrustCenterMetaInfo);
            Certificates.AddRange(importedCertificates);
        }

        public void DeleteTrustCenter(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            this.ConfigManager.DeleteTrustCenterFromConfig(trustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            this.TrustCenterManager.DeleteTrustCenterFile(trustCenterMetaInfo.Name);
            this.TrustCenterManager.DeleteCertificatesOfTrustCenter(this.Certificates, trustCenterMetaInfo.Name);
        }

        public List<TrustCenterMetaInfo> GetTrustCenterHistory()
        {
            return this.Config.TrustCenterMetaInfos;
        }

        public List<Certificate> GetCertificates()
        {
            return this.Certificates;
        }

        #endregion

        #region PrivateMethods
        private bool IsTrustCenterInputValid(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (newTrustCenterName.Length > 29)
                throw new ArgumentException("The entered name is too long.");

            if (newTrustCenterName == String.Empty)
                throw new ArgumentException("The entered name must not be empty.");

            if (!this.TrustCenterManager.DownloadManager.IsUrlExisting(newTrustCenterUrl))
                throw new ArgumentException("The entered Url is not valid.");

            if (this.Config.TrustCenterMetaInfos.Any(tc => tc.Name.Equals(newTrustCenterName)))
                throw new ArgumentException("The entered name is already added.");

            if (this.Config.TrustCenterMetaInfos.Any(tc => tc.TrustCenterUrl.Equals(newTrustCenterUrl)))
                throw new ArgumentException("The entered Url is already added.");

            return true;
        }
        #endregion
    }
}