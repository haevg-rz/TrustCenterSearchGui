using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class TrustCenterManager:ITrustCenterManager
    {
        #region Properties
        internal ITrustCenterImporter ImportManager { get; set; } = new Importer();
        internal ITrustCenterDownloader DownloadManager { get; set; } = new Downloader();

        #endregion

        #region Fields
        private readonly string _dataFolderPath =
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TrustCenterSearch\data\";
        #endregion

        #region ITrustCenterManagerMethods

        public async Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            return await this.DownloadManager.DownloadCertificates(trustCenterMetaInfo, _dataFolderPath);
        }

        public async Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            var importedCertificates = await ImportManager.ImportCertificatesAsync(trustCenterMetaInfo, _dataFolderPath).ConfigureAwait(false);
            return importedCertificates;
        }

        public void DeleteTrustCenterFile(string trustCenterName)
        {
            File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath));
        }

        public void DeleteCertificatesOfTrustCenter(IEnumerable<Certificate> certificates, string trustCenterName)
        {
            certificates = certificates.Where(c => c.TrustCenterName.Equals(trustCenterName));
        }
        #endregion
    }
}
