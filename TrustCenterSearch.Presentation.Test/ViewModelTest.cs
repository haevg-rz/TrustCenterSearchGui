using System.Windows.Data;
using TestSamples;
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

        [Fact]
        public void LoadDataAsyncCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act


            #endregion


            #region Assert


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

        [Fact(DisplayName = "ReloadCertificatesOfTrustCenterCommandExecuteTest")]
        public void ReloadCertificatesOfTrustCenterCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "ReloadTrustCenterHistoryElementTest")]
        public void ReloadTrustCenterHistoryElementTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "AddTrustCenterAsyncCommandExecuteTest")]
        public void AddTrustCenterAsyncCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "DeleteTrustCenterFromHistoryCommandExecuteTest")]
        public void DeleteTrustCenterFromHistoryCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "AddTrustCenterToFilterCommandExecuteTest")]
        public void AddTrustCenterToFilterCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "RemoveTrustCenterFromFilterCommandExecuteTest")]
        public void RemoveTrustCenterFromFilterCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "GetTrustCenterHistoryTest")]
        public void GetTrustCenterHistoryTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

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
