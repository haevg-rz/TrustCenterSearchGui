using System;
using System.Collections.Generic;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core
{
    public class Core
    {
        public string DataFolderPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\data\";
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

            this.DataManager = new DataManager();
            this.Certificates = this.DataManager.GetCertificatesFromAppData(this.Config, DataFolderPath);
            this.DataManager.SetTimeStamp(DataFolderPath);

            this.SearchManager = new SearchManager();
        }
    }
}