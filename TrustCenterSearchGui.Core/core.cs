using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class core
    {
        private static Config Config { get; set; }
        private static List<Certificate> Certificates { get; set; }

        public static void RefreshButton()
        {
            var configManager = new ConfigManager();
            Config = configManager.GetConfig();

            var downloadManager = new DownloadManager(); 
            downloadManager.DownloadDataFromConfic(Config);

            var dataManager = new DataManager();
            Certificates = dataManager.GetCertificateFromAppData(Config);
            dataManager.SetTimeStamp();
        }

        public static ObservableCollection<SearchResultsAndBorder> Searcher(string search)
        {
            var searchManager = new SearchManager();
            var result = searchManager.SearchManagerConnector(search, Certificates);

            return result;
        }
    }
}