using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement.TrustCenters
{
    internal class ImportManager
    {
        #region InternalMethods
        internal async Task<IEnumerable<Certificate>> ImportCertificatesAsync(TrustCenterMetaInfo trustCenterMetaInfo, string dataFolderPath)
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
                NotAfter = c.NotAfter.ToString(CultureInfo.CurrentCulture),
                NotBefore = c.NotBefore.ToString(CultureInfo.CurrentCulture),
                Thumbprint = c.Thumbprint,
                PublicKeyLength = c.PublicKey.ToString().Length,
                TrustCenterName = trustCenterMetaInfo.Name
            }));

            return certificates;
        }

        internal void SetTimeStamp(string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var timeStamp = DateTime.Now;
            File.WriteAllText(filePath + "TimeStamp.json", Convert.ToString(timeStamp));
        }

        #endregion

        #region PrivateMethods
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