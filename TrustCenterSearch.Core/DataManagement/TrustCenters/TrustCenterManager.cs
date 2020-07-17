using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class TrustCenterManager
    {
        #region Properties
        internal ImportManager ImportManager { get; set; } = new ImportManager();
        internal DownloadManager DownloadManager { get; set; } = new DownloadManager();

        #endregion

        #region Fields
        private readonly string _dataFolderPath =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch\data\";
        #endregion

        #region InternalMethods

        internal async Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            return await this.DownloadManager.DownloadCertificates(trustCenterMetaInfo, _dataFolderPath);
        }

        internal async Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            var importedCertificates = await ImportManager.ImportCertificatesAsync(trustCenterMetaInfo, _dataFolderPath).ConfigureAwait(false);
            return importedCertificates;
        }

        internal void DeleteTrustCenterFile(string trustCenterName)
        {
            File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath));
        }

        internal void DeleteCertificatesOfTrustCenter(IEnumerable<Certificate> certificates, string trustCenterName)
        {
            certificates = certificates.Where(c => c.TrustCenterName.Equals(trustCenterName));
        }

        #endregion
    }
}
