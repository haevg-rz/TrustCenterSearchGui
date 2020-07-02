using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.ConfigManager = new ConfigManager();
            this.Config = this.ConfigManager.LoadConfig();
            
            this.ImportManager = new ImportManager();
            this.Certificates = this.ImportManager.ImportCertificatesFromDownloadedTrustCenters(this.Config, DataFolderPath);
            this.ImportManager.SetTimeStamp(DataFolderPath);

            this.DownloadManager = new DownloadManager();
        }

        public void AddTrustCenter(string newTrustCenterName, string newTrustCenterUrl)
        {
            if (newTrustCenterName == string.Empty)
                throw new ArgumentException("The entered name is not valid.");

            if (!DownloadManager.IsUrlExisting(newTrustCenterUrl))
                throw new ArgumentException("The entered Url is not valid.");

            this.ConfigManager.AddTrustCenterToConfig(newTrustCenterName,newTrustCenterUrl,Config);
            this.ConfigManager.SaveConfig(Config);
            this.DownloadManager.DownloadTrustCenter(newTrustCenterName, newTrustCenterUrl, this.DataFolderPath);
            this.Certificates = this.ImportManager.ImportCertificatesFromDownloadedTrustCenters(this.Config, this.DataFolderPath);
        }

        public List<string> LoadTrustCenterHistory()
        {
            return this.Config.TrustCenters.Select(trustCenter => trustCenter.Name).ToList();
        }

        public List<Certificate> LoadCertificates()
        {
            return this.Certificates;
        }
    }
}