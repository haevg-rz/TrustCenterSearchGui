using System;
using System.Collections.Generic;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class Core
    {
        private static string DataFolderPath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            @"\TrustCenterSearch\data\";

        public DataManager DataManager { get; set; }
        public DownloadManager DownloadManager { get; set; }
        public SearchManager SearchManager { get; set; }
        public ConfigManager ConfigManager { get; set; }
        public Config Config { get; set; }
        public List<Certificate> Certificates { get; set; }

        public Core()
        {
            this.ConfigManager = new ConfigManager();
            this.Config = this.ConfigManager.GetConfig();

            this.DownloadManager = new DownloadManager();
            this.DownloadManager.DownloadDataFromConfig(this.Config, DataFolderPath);

            this.DataManager = new DataManager();
            this.Certificates = this.DataManager.GetCertificatesFromAppData(this.Config, DataFolderPath);
            this.DataManager.SetTimeStamp(DataFolderPath);

            this.SearchManager = new SearchManager();
        }

        public void RefreshButtonCommand()
        {
            this.Config = this.ConfigManager.GetConfig();

            this.DownloadManager.DownloadDataFromConfig(this.Config, DataFolderPath);

            this.Certificates = DataManager.GetCertificatesFromAppData(Config, DataFolderPath);
        }
    }
}