using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearch.Core.DataManagement
{
    internal class ImportManager
    {
        #region InternalMethods

        internal async Task<List<Certificate>> ImportCertificatesFromDownloadedTrustCenter(
            List<Certificate> certificates, TrustCenter trustCenter, string filePath)
        {
            var certificatesTxt = await ReadFile(trustCenter, filePath).ConfigureAwait(false);

            var cer = from certificateTxt in certificatesTxt
                select new X509Certificate2(Convert.FromBase64String(certificateTxt));

            certificates.AddRange(cer.Select(c => new Certificate()
            {
                Subject = c.Subject,
                Issuer = c.Issuer,
                SerialNumber = c.SerialNumber,
                NotAfter = c.NotAfter,
                NotBefore = c.NotBefore,
                Thumbprint = c.Thumbprint
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

        private static async Task<string[]> ReadFile(TrustCenter trustCenter, string filePath)
        {
            byte[] result;
            using (var stream = File.Open(filePath + trustCenter.Name + @".txt", FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return System.Text.Encoding.ASCII.GetString(result).
                Split(new[] { Environment.NewLine + Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion
    }
}