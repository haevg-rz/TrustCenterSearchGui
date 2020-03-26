using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class Core
    {
        private const int RefreshAfterDays = 30;

        private static string FilePath { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            @"\TrustCenterSearch\data\";

        private static Config Config { get; set; }
        private static List<Certificate> Certificates { get; set; }

        public static void RefreshButton()
        {
            // TODO Why not keep the instance?
            var configManager = new ConfigManager();
            Config = configManager.GetConfig();

            var dataManager = new DataManager();
            if ((DateTime.Now - dataManager.GetTimeStamp(FilePath)).TotalDays > RefreshAfterDays)
            {
                var downloadManager = new DownloadManager();
                downloadManager.DownloadDataFromConfig(Config, FilePath);

                dataManager.SetTimeStamp(FilePath);
            }

            Certificates = dataManager.GetCertificateFromAppData(Config, FilePath);
        }

        public static ObservableCollection<SearchResultsAndBorder> GetFilterdList(string search)
        {
            var searchManager = new SearchManager();
            var result = searchManager.SearchManagerConnector(search, Certificates);

            return result;
        }
    }
}