﻿using System;
using System.Threading.Tasks;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Statistics
{
    public abstract class Statistics : Base.ViewModel
    {
        private bool _isLoaded;
        private bool _isLoading;
        private bool _showNoData;
        private bool _showPlot;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
                NotifyPropertyChanged("IsLoaded");
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public bool ShowPlot
        {
            get
            {
                return _showPlot;
            }
            set
            {
                _showPlot = value;
                NotifyPropertyChanged("ShowPlot");
            }
        }

        public async void LoadStatistics()
        {
            IsLoading = true;
            await TaskEx.Run((Action)ProcedeLoad);

            IsLoading = false;
            ShowPlot = true;
            IsLoaded = true;
        }

        protected abstract void ProcedeLoad();
    }
}