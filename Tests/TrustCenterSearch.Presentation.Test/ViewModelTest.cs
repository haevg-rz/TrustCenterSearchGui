using Moq;
using System.Windows.Data;
using TestSamples;
using TrustCenterSearch.Core;
using TrustCenterSearch.Core.Models;
using TrustCenterSearch.Presentation;
using Xunit;

namespace TrustCenterSearchPresentation.Test
{
    public class ViewModelTest
    {

        [Fact(DisplayName = "CopySearchResultToClipboardCommandExecuteTest")]
        public void CopySearchResultToClipboardCommandExecuteTest()
        {
            #region Arrange

            //var cer = new Certificate()
            //{
            //    SerialNumber = "TestNummer",
            //    Issuer = "TestIssuer",
            //    NotAfter = "TestStartDate",
            //    NotBefore = "TestEndDate",
            //    PublicKeyLength = "TestLength",
            //    Thumbprint = "TestThumbprint",
            //    TrustCenterName = "TestName",
            //    Subject = "Test"
            //};

            //string jsonstring = JsonConvert.SerializeObject(cer, Formatting.Indented);
            //var test = "null";

            #endregion

            #region Act

            //var viewModel = new ViewModel();
            //viewModel.CopySearchResultToClipboardCommandExecute(cer);

            #endregion

            #region Assert

            //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            //var t = new Thread((ThreadStart)(() =>
            //{
            //    test = Clipboard.GetText();
            //}));
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
            //t.Join();
            
            //Assert.Equal(test, jsonstring);

            #endregion

        }

        [Theory]
        [InlineData("Auto", "0")]
        [InlineData("0", "Auto")]
        public void CollapseSidebarCommandExecuteTest(string setMenuWidth, string expectedMenuWidth)
        {
            #region Arrange

            var view = new ViewModel();
            view.MenuWidth = setMenuWidth;

            #endregion


            #region Act

            view.CollapseSidebarCommandExecute();

            #endregion


            #region Assert

            Assert.Equal(view.MenuWidth, expectedMenuWidth);

            #endregion

        }


        [Fact(DisplayName = "GetTrustCenterHistoryTest")]
        public void GetTrustCenterHistoryTest()
        {
            #region Arrange

            var viewModel = new ViewModel();

            var moqCore = new Mock<Core>();
            moqCore.Setup(m => m.GetTrustCenterHistory()).Returns(Samples.ProvideSampleMetaInfos);

            viewModel.Core = moqCore.Object;

            #endregion


            #region Act

            var result = viewModel.GetTrustCenterHistory();

            #endregion


            #region Assert

            Assert.Equal(3, result.Count);

            #endregion

        }

        [Theory]
        [ClassData(typeof(FilterTestData))]
        public void FilterTest(Certificate certificate,bool expectedBoolean,string searchBarInputForTesting)
        {
            #region Arrange

            var viewModel = new ViewModel();

            viewModel.CertificatesCollectionView = CollectionViewSource.GetDefaultView(Samples.ProvideSampleCertificates());
            viewModel.CertificatesCollectionView.Filter = viewModel.Filter;

            viewModel.SearchBarInput = searchBarInputForTesting;

            viewModel.TrustCenterHistory = Samples.ProvideSampleListOfTrustCenterHistoryElements();
            #endregion


            #region Act

            var result = viewModel.Filter(certificate);
            
            #endregion


            #region Assert

            Assert.Equal(result, expectedBoolean);

            #endregion

        }
    }
}
