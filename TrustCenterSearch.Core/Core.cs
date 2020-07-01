using System;
using System.Collections.Generic;
using System.Linq;
using TrustCenterSearch.Core.DataManagement;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core
{
    public class Core
    {
        public string DataFolderPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\data\";
        public ImportManager ImportManager { get; set; }
        public DownloadManager DownloadManager { get; set; }
        public SearchManager SearchManager { get; set; }
        public ConfigManager ConfigManager { get; set; }
        public Config Config { get; set; }
        public List<Certificate> Certificates { get; set; }

        public Core()
        {
            this.ConfigManager = new ConfigManager();
            this.Config = this.ConfigManager.LoadConfig();
            
            this.ImportManager = new ImportManager();
            this.Certificates = this.ImportManager.ImportCertificatesFromDownloadedTrustCenters(this.Config, DataFolderPath);
            this.ImportManager.SetTimeStamp(DataFolderPath);

            this.DownloadManager = new DownloadManager();
            this.SearchManager = new SearchManager();
        }

        public List<SearchResultsAndBorder> ExecuteSearch(string searchInput)
        {
            return this.SearchManager.GetSearchResults(searchInput, this.Certificates);
        }

        public void AddTrustCenter(string newTrustCenterName, string newTrustCenterUrl)
        {
            this.Config = this.ConfigManager.AddTrustCenterToConfig(newTrustCenterName, newTrustCenterUrl, this.Config);
            this.DownloadManager.DownloadTrustCenter(newTrustCenterName, newTrustCenterUrl, this.DataFolderPath);
            this.Certificates = this.ImportManager.ImportCertificatesFromDownloadedTrustCenters(this.Config, this.DataFolderPath);
        }

        public List<TrustCenterHistoryElement> LoadTrustCenterHistory()
        {
            return Config.TrustCenters.Select(trustCenter => new TrustCenterHistoryElement(trustCenter.Name)).ToList();
        }
    }
}