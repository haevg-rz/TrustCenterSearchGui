using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.DataManagement;
using TrustCenterSearch.Core.Models;

[assembly: InternalsVisibleTo("TrustCenterSearchCore.Test")]

namespace TrustCenterSearch.Core
{
    public class Core
    {
        internal string DataFolderPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\data\";
        internal ImportManager ImportManager { get; set; }
        internal DownloadManager DownloadManager { get; set; }
        internal ConfigManager ConfigManager { get; set; }
        internal Config Config { get; set; }
        internal List<Certificate> Certificates { get; set; }

        public Core()
        {
            Certificates = new List<Certificate>();

            this.ConfigManager = new ConfigManager();
            this.ImportManager = new ImportManager();

            this.Config = this.ConfigManager.LoadConfig();
            this.DownloadManager = new DownloadManager();
        }

        #region PublicMethods

        public async Task ImportAllCertificatesFromTrustCenters()
        {
            var importTasks = new List<Task<List<Certificate>>>();
            foreach (var trustCenter in Config.TrustCenters)
            {
                importTasks.Add(this.ImportManager.ImportCertificatesFromDownloadedTrustCenter(
                    this.Certificates, trustCenter, DataFolderPath));
            }
            await Task.WhenAll(importTasks);
        }

        public async Task AddTrustCenter(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (!this.IsTrustCenterInputValid(newTrustCenterName, newTrustCenterUrl))
                return;

            var newTrustCenter = this.ConfigManager.AddTrustCenterToConfig(newTrustCenterName, newTrustCenterUrl, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            await this.DownloadManager.DownloadTrustCenter(newTrustCenterName, newTrustCenterUrl, this.DataFolderPath);
            this.Certificates = await this.ImportManager.ImportCertificatesFromDownloadedTrustCenter(this.Certificates, newTrustCenter, this.DataFolderPath);
        }

        public async Task DeleteTrustCenter(string trustCenterName)
        {
            this.ConfigManager.DeleteTrustCenterFromConfig(trustCenterName, this.Config);
            this.ConfigManager.SaveConfig(this.Config);
            this.DeleteTrustCenterFile(trustCenterName);
            this.Certificates.Clear();
            await this.ImportAllCertificatesFromTrustCenters();
        }

        public List<string> LoadTrustCenterHistory()
        {
            return this.Config.TrustCenters.Select(trustCenter => trustCenter.Name).ToList();
        }

        public List<Certificate> GetCertificates()
        {
            return this.Certificates;
        }

        #endregion

        #region PrivateMethods

        private bool IsTrustCenterInputValid(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (newTrustCenterName == string.Empty)
                throw new ArgumentException("The entered name must not be empty.");

            if (!this.DownloadManager.IsUrlExisting(newTrustCenterUrl))
                throw new ArgumentException("The entered Url is not valid.");

            if (this.Config.TrustCenters.Any(tc => tc.Name.Equals(newTrustCenterName)))
                throw new ArgumentException("The entered name is already added.");

            if (this.Config.TrustCenters.Any(tc => tc.TrustCenterUrl.Equals(newTrustCenterUrl)))
                throw new ArgumentException("The entered Url is already added.");

            return true;
        }

        private void DeleteTrustCenterFile(string trustCenterName)
        {
            File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this.DataFolderPath));
        }
        #endregion
    }
}