using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Interfaces.TrustCenters;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class Importer:ITrustCenterImporter
    {
        #region ITrustCenterImporterMethods
        public async Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath)
        {
            var certificates = new HashSet<Certificate>();

            var certificatesTxt = await ReadFileAsync(trustCenterMetaInfo, dataFolderPath).ConfigureAwait(false);

            var cer = from certificateTxt in certificatesTxt
                select new X509Certificate2(Convert.FromBase64String(certificateTxt));

            certificates.UnionWith(cer.Select(c => new Certificate()
            {
                Subject = c.Subject,
                Issuer = c.Issuer,
                SerialNumber = c.SerialNumber,
                NotAfter = c.NotAfter.Date.ToShortDateString(),
                NotBefore = c.NotBefore.Date.ToShortDateString(),
                Thumbprint = c.Thumbprint,
                PublicKeyLength = c.PublicKey.ToString().Length.ToString(),
                TrustCenterName = trustCenterMetaInfo.Name
            }));

            return certificates;
        }
        #endregion

        #region PrivateStaticMethods
        private static async Task<string[]> ReadFileAsync(TrustCenterMetaInfo trustCenter, string dataFolderPath)
        {
            byte[] result;
            using (var stream = File.Open(dataFolderPath + trustCenter.Name + @".txt", FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return System.Text.Encoding.UTF8.GetString(result).
                Split(new[] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion
    }
}