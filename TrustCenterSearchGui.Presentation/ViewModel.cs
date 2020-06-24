﻿using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TrustCenterSearchGui.Core;
using TrustCenterSearchGui.Core.Models;

namespace TrustCenterSearchGui.Presentation
{
    public class ViewModel : ViewModelBase
    {

        public ViewModel()
        {
            this.SearchCalculation();
            this.RefreshButton = new RelayCommand(RefreshAndSearch);
        }

        public RelayCommand RefreshButton { get; set; }
        public RelayCommand CollapseButton { get; set; }

        private string search;
        public string Search 
        { 
            get => this.search;
            set
            {
                base.Set(ref this.search, value);
                this.SearchCalculation();
            }
        }

        private ObservableCollection<SearchResultsAndBorder> certificateSearchResultList = new ObservableCollection<SearchResultsAndBorder>();
        public ObservableCollection<SearchResultsAndBorder> CertificateSearchResultList
        { 
            get => this.certificateSearchResultList; 
            set => base.Set(ref this.certificateSearchResultList, value);
        }

        private void SearchCalculation()
        {
            var searchResult = TrustCenterSearchGui.Core.Intersection.Searcher(this.Search);

            this.CertificateSearchResultList = searchResult;
        }

        private void RefreshAndSearch()
        {
            TrustCenterSearchGui.Core.Intersection.RefreshButton();
            SearchCalculation();
            if (Intersection.ConfigIsEmpty())
                MessageBox.Show("There are no TrustCenters added in the Config.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
