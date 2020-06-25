using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class Core
    {
        private static string FilePath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            @"\TrustCenterSearch\data\";

        private static Config Config { get; set; }
        private static List<Certificate> Certificates { get; set; }

        public static void RefreshButtonCommand()
        {
            var configManager = new ConfigManager();
            Config = configManager.GetConfig();

            var downloadManager = new DownloadManager(); 
            downloadManager.DownloadDataFromConfig(Config, FilePath);

            var dataManager = new DataManager();
            Certificates = dataManager.GetCertificatesFromAppData(Config, FilePath);
            dataManager.SetTimeStamp(FilePath);
        }

        public static List<SearchResultsAndBorder> Searcher(string search)
        {
            var searchManager = new SearchManager();
            var result = searchManager.SearchManagerConnector(search, Certificates);

            return result;
        }

        public static bool ConfigIsEmpty()
        {
            return Config.TrustCenters.Count == 0;
        }
    }
}