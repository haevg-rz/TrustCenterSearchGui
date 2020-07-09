using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class TrustCenterManager
    {
        #region Properties

        internal ImportManager ImportManager { get; set; }
        internal DownloadManager DownloadManager { get; set; }
        internal List<TrustCenter> TrustCenters { get; set; }

        #endregion

        #region Fields
        private readonly string _dataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\data\";
        #endregion

        internal TrustCenterManager()
        {
            this.DownloadManager = new DownloadManager();
            this.ImportManager = new ImportManager();
            this.TrustCenters = new List<TrustCenter>();
        }

        #region InternalMethods

        internal async Task DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            await this.DownloadManager.DownloadCertificates(trustCenterMetaInfo, _dataFolderPath);
        }

        internal async Task ImportCertificates(TrustCenterMetaInfo trustCenterMetaInfo, List<Certificate> certificates)
        {
            var importedCertificates = await ImportManager.ImportCertificates(trustCenterMetaInfo, _dataFolderPath).ConfigureAwait(false);
            this.TrustCenters.Add(new TrustCenter(trustCenterMetaInfo, importedCertificates));
            certificates.AddRange(importedCertificates);
        }

        internal void DeleteTrustCenterFile(string trustCenterName)
        {
            File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath));
        }

        #endregion
    }
}
