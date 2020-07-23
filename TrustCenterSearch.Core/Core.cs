using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.DataManagement.Configuration;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using TrustCenterSearch.Core.Interfaces.Configuration;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

[assembly: InternalsVisibleTo("TrustCenterSearchCore.Test")]

namespace TrustCenterSearch.Core
{
    public class Core
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
            var importTasks = this.Config.TrustCenterMetaInfos.Select(trustCenterMetaInfo => this.TrustCenterManager.ImportCertificatesAsync(trustCenterMetaInfo)).ToList();
            await Task.WhenAll(importTasks);

            foreach (var importTask in importTasks)
            {
                this.Certificates.AddRange(importTask.Result);
            }

            return this.Certificates;
        }
        public async Task<TrustCenterMetaInfo> AddTrustCenterAsync(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (!this.IsTrustCenterInputValid(newTrustCenterName, newTrustCenterUrl))
                return null;

            var newTrustCenterMetaInfo = new TrustCenterMetaInfo(newTrustCenterName, newTrustCenterUrl, DateTime.Now);
            this.ConfigManager.AddTrustCenterToConfig(newTrustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            await this.TrustCenterManager.DownloadCertificatesAsync(newTrustCenterMetaInfo);
            var importedCertificates =  await this.TrustCenterManager.ImportCertificatesAsync(newTrustCenterMetaInfo);
            this.Certificates.AddRange(importedCertificates);

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
            if (newTrustCenterName.Length > 24)
                throw new ArgumentException("The entered name is too long.");

            if (newTrustCenterName == String.Empty)
                throw new ArgumentException("The entered name must not be empty.");

            if (!Downloader.IsUrlExisting(newTrustCenterUrl))
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