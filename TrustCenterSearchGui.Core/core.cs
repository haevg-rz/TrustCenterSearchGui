using System;
using System.Collections.Generic;
using System.Text;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Core
{
    public class Core
    {
        public static Config Config { get; set; }
        public static List<Certificate> Certificates { get; set; }

        public void FunctionCollection()
        {
            var configManager = new ConfigManager();
            Config = configManager.GetConfig();

            var downloadManager = new DownloadManager(); 
            downloadManager.DownloadDataFromConfic(Config);

            var dataManager = new DataManager();
            Certificates = dataManager.GetCertificateFromAppData(Config);
        }
    }
}