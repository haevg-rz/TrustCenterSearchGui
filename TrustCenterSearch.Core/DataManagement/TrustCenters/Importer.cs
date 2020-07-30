using System;
using System.Collections.Generic;
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
            try
            {
                var fileContent = await ReadFileAsync(trustCenterMetaInfo, dataFolderPath).ConfigureAwait(false);

                var certificates = GetCertificatesFromByteArray(trustCenterMetaInfo, fileContent);

                return certificates;
            }
            catch (Exception e)
            {
                return new HashSet<Certificate>();
            }
        }
        #endregion

        #region PrivateStaticMethods
        private static async Task<byte[]> ReadFileAsync(TrustCenterMetaInfo trustCenter, string dataFolderPath)
        {
            byte[] result;
            using (var stream = File.Open(dataFolderPath + trustCenter.Name + @".txt", FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return result;
        }

        internal static HashSet<Certificate> GetCertificatesFromByteArray(TrustCenterMetaInfo trustCenterMetaInfo, byte[] fileContent)
        {
            var certificatesTxt = System.Text.Encoding.UTF8.GetString(fileContent).Split(
                new[] { Environment.NewLine + Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            var cer = from certificateTxt in certificatesTxt
                select new X509Certificate2(Convert.FromBase64String(certificateTxt));

            var certificates = new HashSet<Certificate>();

            certificates.UnionWith(cer.Select(c => new Certificate()
            {
                Subject = GetSubjectElementsToDisplay(c.Subject),
                SerialNumber = c.SerialNumber,
                NotAfter = c.NotAfter.Date.ToShortDateString(),
                NotBefore = c.NotBefore.Date.ToShortDateString(),
                Thumbprint = c.Thumbprint,
                PublicKeyLength = c.PublicKey.Key.KeySize.ToString(),
                TrustCenterName = trustCenterMetaInfo.Name
            }));
            return certificates;
        }

        internal static string GetSubjectElementsToDisplay(string argSubject)
        {
            var subjectElements = argSubject.Split(',');

            var subjectElementsToDisplay = subjectElements.Where(x => x.Contains("CN=") || x.Contains("OU="));

            var newSubject = subjectElementsToDisplay.Aggregate(String.Empty, (current, element) => current + element);

            return newSubject.Equals(String.Empty) ? "No Subjectinfo available" : newSubject;
        }
        #endregion
    }
}