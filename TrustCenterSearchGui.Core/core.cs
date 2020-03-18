using System;
using System.Text;

namespace TrustCenterSearchGui.Core
{
    public class Core
    {
        public static Config Config { get; set; }

        public void CoreRoutine()
        {
            var configManager = new ConfigManager();
            Config = configManager.GetConfig();

            var downloadManager = new DownloadManager();
            downloadManager.DownloadDataFromConfic(Config);
        }
    }
}