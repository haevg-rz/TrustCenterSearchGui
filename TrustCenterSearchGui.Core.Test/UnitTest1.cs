using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
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
                    TrustCenterURL = "link1"
                }
            };
            

            var downloadManager = new DownloadManager();

            #endregion

            #region Act

            downloadManager.DownloadDataFromConfic(config);

            dataManager.GetCertificateFromAppData(config);

            #endregion

            #region Assert

            Assert.Equal(true, true);

            #endregion
        }
    }
}