using System.Runtime.CompilerServices;
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
        internal IEnumerable<Certificate> Certificates { get; set; } = new HashSet<Certificate>();
        #endregion

        #region Constructor
        public Core()
        {
            this.Config = this.ConfigManager.LoadConfig();
        }
        #endregion

        #region PublicMethods
        public async Task ImportAllCertificatesFromTrustCentersAsync()
        {
            var importTasks = Config.TrustCenterMetaInfos.Select(trustCenterMetaInfo => this.TrustCenterManager.ImportCertificatesAsync(trustCenterMetaInfo)).ToList();
            await Task.WhenAll(importTasks);

            foreach (var importTask in importTasks)
            {
                Certificates = Certificates.Union(importTask.Result);
            }
        }

        public async Task AddTrustCenterAsync(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (!this.IsTrustCenterInputValid(newTrustCenterName, newTrustCenterUrl))
                return;

            var newTrustCenterMetaInfo = new TrustCenterMetaInfo(newTrustCenterName, newTrustCenterUrl);
            this.ConfigManager.AddTrustCenterToConfig(newTrustCenterMetaInfo, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            await this.TrustCenterManager.DownloadCertificatesAsync(newTrustCenterMetaInfo);
            var importedCertificates =  await this.TrustCenterManager.ImportCertificatesAsync(newTrustCenterMetaInfo);
            Certificates = Certificates.Union(importedCertificates);
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

        public IEnumerable<Certificate> GetCertificates()
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