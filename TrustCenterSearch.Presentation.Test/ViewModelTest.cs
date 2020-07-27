using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using Newtonsoft.Json;
using Xunit;
using TrustCenterSearch.Core.Models;

namespace TrustCenterSearchPresentation.Test
{
    public class ViewModelTest
    {
        [Fact(DisplayName = "CopySearchResultToClipboardCommandExecuteTest")]
        public void CopySearchResultToClipboardCommandExecuteTest()
        {
            #region Arrange

            var cer = new Certificate()
            {
                Subject = "Test"
            };

            var jsonString = JsonConvert.SerializeObject(cer, Formatting.Indented);
            var test = Convert.ToString(jsonString);
            #endregion

            #region Act

            TrustCenterSearch.Presentation.ViewModel.CopySearchResultToClipboardCommandExecute(cer);

            #endregion

            #region Assert
            
            Assert.Equal(test,Clipboard.GetText());

            #endregion

        }

        [Fact(DisplayName = "LoadDataAsyncCommandExecuteTest")]
        public void LoadDataAsyncCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }

        [Fact(DisplayName = "CollapseSidebarCommandExecuteTest")]
        public void CollapseSidebarCommandExecuteTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

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

        [Fact(DisplayName = "FilterTest")]
        public void FilterTest()
        {
            #region Arrange

            #endregion


            #region Act

            #endregion


            #region Assert

            #endregion

        }
    }
}
