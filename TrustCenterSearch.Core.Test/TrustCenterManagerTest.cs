
using System.Linq;
using TrustCenterSearch.Core.DataManagement.TrustCenters;
using Xunit;

namespace TrustCenterSearchCore.Test
{
    public class TrustCenterManagerTest
    {
        [Fact(DisplayName = "ImportCertificatesAsyncTest")]
        public void ImportCertificatesAsyncTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }
        [Fact(DisplayName = "ReadFileAsyncTest")]
        public void ReadFileAsyncTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }
        [Fact(DisplayName = "GetSubjectElementsToDisplayTest")]
        public void GetSubjectElementsToDisplayTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "DeleteCertificatesOfTrustCenterTest")]
        public void DeleteCertificatesOfTrustCenterTest()
        {
            #region Arrange

            var sample = Samples.ProvideSampleCertificates();

            var trustCenterManager = new TrustCenterManager();

            var trustCenterToDelete = sample.FirstOrDefault().TrustCenterName;

            #endregion


            #region Act

            trustCenterManager.DeleteCertificatesOfTrustCenter(sample, trustCenterToDelete);

            #endregion


            #region Assert

            Assert.DoesNotContain(sample, certificate => certificate.TrustCenterName.Equals(trustCenterToDelete));

            #endregion

        }
    }
}
