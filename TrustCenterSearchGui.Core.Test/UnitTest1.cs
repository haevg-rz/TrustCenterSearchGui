using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using TrustCenterSearchGui.Core.Models;
using Xunit;

namespace TrustCenterSearchGui.Core.Test
{
    public class UnitTest1
    {
        [Fact(DisplayName = "test")]
        public void ReadConfig()
        {
            #region Arrange

            var dataManager = new DataManager();
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

            var result = dataManager.GetCertificateFromAppData(config, filePath);

            #endregion

            #region Assert

            var jsonString = JsonConvert.SerializeObject(result, Formatting.Indented);
            System.IO.File.WriteAllText(filePath + @"\ResultCertificate.JSON", jsonString);

            object.Equals(filePath +@"\AspectedResultCertificate.JSON", filePath + @"\ResultCertificate.JSON");

            #endregion
        }
    }
}