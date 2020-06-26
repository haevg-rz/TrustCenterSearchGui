using System.IO;
using System.Net;
using System.Text;

namespace TrustCenterSearchGui.Core
{
    public class DownloadManager
    {
        public void DownloadDataFromConfig(Config config, string filePath)
        {
            var dataManager = new DataManager();
            dataManager.CreateDirectoryIfMissing(filePath);

            var client = new WebClient();

            foreach (var trustCenter in config.TrustCenters)
            {
                var data = client.DownloadData(trustCenter.TrustCenterURL);
                var str = Encoding.UTF8.GetString(data);
                File.WriteAllText(GetFilePath(trustCenter.Name, filePath), str);
            }
        }

        private string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }
    }
}