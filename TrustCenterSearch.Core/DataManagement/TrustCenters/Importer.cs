using System;
using System.Collections.Concurrent;
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
            catch (Exception)
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

        private static HashSet<Certificate> GetCertificatesFromByteArray(TrustCenterMetaInfo trustCenterMetaInfo, byte[] fileContent)
        {
            var certificatesTxt = System.Text.Encoding.UTF8.GetString(fileContent).Split(
                new[] { Environment.NewLine + Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            var collection = new BlockingCollection<(X509Certificate2 cert, string keySize)>(certificatesTxt.Length);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
            };

            Parallel.ForEach(certificatesTxt, parallelOptions, (s, state) =>
            {
                 var cert = new X509Certificate2(Convert.FromBase64String(s));
                 var keySize = cert.PublicKey.Key.KeySize.ToString();

                 collection.Add((cert, keySize));
            });

            var cer = certificatesTxt.Select(certificateTxt =>
                new X509Certificate2(Convert.FromBase64String(certificateTxt)));

            var certificates = new HashSet<Certificate>();

            certificates.UnionWith(collection.Select(c =>
            {
                var certificate = new Certificate();
                certificate.Subject = GetSubjectElementsToDisplay(c.cert.Subject);
                certificate.SerialNumber = c.cert.SerialNumber;
                certificate.NotAfter = c.cert.NotAfter.Date.ToShortDateString();
                certificate.NotBefore = c.cert.NotBefore.Date.ToShortDateString();
                certificate.Thumbprint = c.cert.Thumbprint;
                certificate.PublicKeyLength = c.keySize;
                certificate.TrustCenterName = trustCenterMetaInfo.Name;
                return certificate;
            }));
            return certificates;
        }

        internal static string GetSubjectElementsToDisplay(string argSubject)
        {
            var subjectElements = argSubject.Split(',');

            var subjectElementsToDisplay = subjectElements.Where(x => x.Contains("CN=") || x.Contains("OU="));

            var newSubject = subjectElementsToDisplay.Aggregate(String.Empty, (current, element) => current + element);

            return newSubject.Equals(String.Empty) ? argSubject : newSubject;
        }
        #endregion
    }
}