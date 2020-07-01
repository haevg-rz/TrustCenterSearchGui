using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.DataManagement;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class GetCertificateFromStringTest
    {
        [Fact(DisplayName = "test")]
        public void ReadConfig()
        {
            #region Arrange

            var dataManager = new ImportManager();
            var config = new Config();
            config.TrustCenters = new List<TrustCenter>()
            {
                new TrustCenter()
                {
                    Name = "MyGreatTrustCenter",
                    TrustCenterURL = "URL"
                }
            };
            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                           @"\TrustCenterSearch\test\";

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var downloadManager = new DownloadManager();

            #endregion

            #region Act

            var result = dataManager.ImportCertificatesFromDownloadedTrustCenters(config, filePath);

            #endregion

            #region Assert

            var jsonString = JsonConvert.SerializeObject(result, Formatting.Indented);
            System.IO.File.WriteAllText(filePath + @"\ResultCertificate.json", jsonString);

            object.Equals(filePath +@"\AspectedResultCertificate.JSON", filePath + @"\ResultCertificate.json");

            #endregion
        }
    }
}