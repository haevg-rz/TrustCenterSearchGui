﻿using System;
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

        public virtual async Task<byte[]> DownloadCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            return await this.DownloadManager.DownloadCertificates(trustCenterMetaInfo, _dataFolderPath);
        }

        public virtual async Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo)
        {
            return await this.ImportManager.ImportCertificatesAsync(trustCenterMetaInfo, _dataFolderPath).ConfigureAwait(false);
        }
        public virtual async Task<List<Certificate>> ImportCertificatesAsync(List<TrustCenterMetaInfo> trustCenterMetaInfos, List<Certificate> certificates)
        {
            var importTasks = trustCenterMetaInfos.Select(this.ImportCertificatesAsync).ToList();
            await Task.WhenAll(importTasks);

            foreach (var importTask in importTasks) certificates.AddRange(importTask.Result);

            return certificates;
        }

        public virtual bool DeleteTrustCenterFile(string trustCenterName)
        {
            if(File.Exists(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath)))
                File.Delete(this.DownloadManager.GetFilePath(trustCenterName, this._dataFolderPath));

            return true;
        }

        public List<Certificate> DeleteCertificatesOfTrustCenter(List<Certificate> certificates, string trustCenterName)
        {
            certificates.RemoveAll(c => c.TrustCenterName.Equals(trustCenterName));
            return certificates;
        }
        #endregion
    }
}
