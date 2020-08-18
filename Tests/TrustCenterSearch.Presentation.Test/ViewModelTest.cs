using System.Windows.Data;
using Moq;
using TestSamples;
using TrustCenterSearch.Core.Models;
using Xunit;

namespace TrustCenterSearch.Presentation.Test
{
    public class ViewModelTest
    {
        [Theory]
        [InlineData("Auto", "0")]
        [InlineData("0", "Auto")]
        public void CollapseSidebarCommandExecuteTest(string setMenuWidth, string expectedMenuWidth)
        {
            #region Arrange

            var view = new ViewModel {MenuWidth = setMenuWidth};

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

            var moqCore = new Mock<Core.Core>();
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
        public void FilterTest(Certificate certificate, bool expectedBoolean, string searchBarInputForTesting)
        {
            #region Arrange

            var viewModel = new ViewModel();

            viewModel.CertificatesCollectionView =
                CollectionViewSource.GetDefaultView(Samples.ProvideSampleCertificates());
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