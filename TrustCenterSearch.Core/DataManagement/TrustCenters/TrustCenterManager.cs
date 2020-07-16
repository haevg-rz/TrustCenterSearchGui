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

        #endregion

        #region Fields
        private readonly string _dataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TrustCenterSearch\data\";
        #endregion

        #region Constructor
        internal TrustCenterManager()
        {
            this.DownloadManager = new DownloadManager();
            this.ImportManager = new ImportManager();
        }
        #endregion

        #region InternalMethods

        internal async Task DownloadCertificates(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            await this.DownloadManager.DownloadCertificates(trustCenterMetaInfo, _dataFolderPath);
        }

        internal async Task<List<Certificate>> ImportCertificates(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            var importedCertificates = await ImportManager.ImportCertificates(trustCenterMetaInfo, _dataFolderPath).ConfigureAwait(false);
            return importedCertificates;
        }

        internal void DeleteTrustCenterFile(string trustCenterName)
        {
            File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath));
        }

        internal void DeleteCertificatesOfTrustCenter(List<Certificate> certificates, string trustCenterName)
        {
            certificates.RemoveAll(c => c.TrustCenterName.Equals(trustCenterName));
        }

        #endregion
    }
}
