﻿using System;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;

using Gymme.ViewModel.AddEdit;

namespace Gymme.View
{
    public partial class AddEditPage : PhoneApplicationPage
    {
        public const string VariantAddWorkout = "addworkout";

        private AddEditVM _viewModel;

        public AddEditPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string target;
            if(!NavigationContext.QueryString.TryGetValue("navtgt", out target))
            {
                NavigationManager.GoBack();
            }

            DataContext = _viewModel = GetDataContext(target);
        }

        private AddEditVM GetDataContext(string target)
        {
            switch (target)
            {
                case VariantAddWorkout: 
                    return new AddEditWorkoutVM { Control = new AEWorkout() };
                default: 
                    NavigationManager.GoBack();
                    return null;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ((IAEView)_viewModel.Control).UpdateDataSources();
            _viewModel.Commit();
            NavigationManager.GoBack();
        }
    }
}