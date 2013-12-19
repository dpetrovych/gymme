﻿using System.Globalization;
using System.Linq;
using Gymme.View;
using System;
using System.Windows.Navigation;
using Gymme.View.Pages;
using AEC = Gymme.View.Controls.AddEditChooser;

namespace Gymme
{
    public static class NavigationManager
    {
        private const string AddEditPagePath = "/View/Pages/AddEditPage.xaml";
        private const string WorkoutPagePath = "/View/Pages/WorkoutPage.xaml";
        private const string ExercisePagePath = "/View/Pages/ExercisePage.xaml";
        private const string TrainingPagePath = "/View/Pages/TrainingPage.xaml";
        private const string ExecutionPagePath = "/View/Pages/ExecutionPage.xaml";
        private const string ExercisesSelectPath = "/View/Pages/ExercisesSelectPage.xaml";

        private static NavigationService NavigationService;

        public static string GoBackParams { get; set; }

        public static void GotoAddWorkout()
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AEC.Variant.AddWorkout));
        }

        public static void GotoEditWorkout(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AEC.Variant.EditWorkout, Id(id)));
        }

        public static void GotoWorkoutPage(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(WorkoutPagePath, "none", Id(id)));
        }

        public static void GotoExercisesSelectPage(long workoutId)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(ExercisesSelectPath, "none", Param(AEC.Param.WorkoutId, workoutId)));
        }

        public static void GotoAddExercisePage(long workoutId)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AEC.Variant.AddExercise, Param(AEC.Param.WorkoutId, workoutId)));
        }

        public static void GotoAddExercisePage(long workoutId, long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AEC.Variant.AddExercise, Id(id) + Param(AEC.Param.WorkoutId, workoutId)));
        }

        public static void GotoEditExercise(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(AddEditPagePath, AEC.Variant.EditExercise, Id(id)));
        }

        public static void GotoExercisePage(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(ExercisePagePath, "none", Id(id)));
        }

        public static void GotoTrainingPage(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(TrainingPagePath, TrainingPage.ByTraining, Id(id)));
        } 
        
        public static void GotoTrainingPageFromWorkout(long id, bool startNew)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(TrainingPagePath, startNew ? TrainingPage.FromWorkoutStart : TrainingPage.FromWorkoutContinue , Id(id)));
        }

        public static void GotoExecutePage(long id)
        {
            InitializeNavigation();
            NavigationService.Navigate(BuildUri(ExecutionPagePath, "none", Id(id)));
        }

        public static void GoBack(string parameters = null, int times = 1)
        {
            while (times > 1 && NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
                times--;
            }

            if (NavigationService.CanGoBack) 
            {
                NavigationService.GoBack();
            }

            GoBackParams = parameters ?? string.Empty;
        }

        public static void SetNavigationService(NavigationService ns)
        {
            if (ns == null)
            {
                throw new InvalidOperationException("NavigationService cannot be null");
            }

            NavigationService = ns;
        }

        private static void InitializeNavigation()
        {
            GoBackParams = null;
        }

        #region BuildUri

        private static string Id(long id)
        {
            return Param("id", id);
        }

        private static string Param(string paramName, object param)
        {
            return string.Format(CultureInfo.InvariantCulture, "&{0}={1}", paramName, param);
        }

        private static Uri BuildUri(string path, string navtgt, string query)
        {
            return new Uri(string.Format("{0}{1}", GetBaseUriString(path, navtgt), query), UriKind.Relative);
        }

        private static Uri BuildUri(string path, string navtgt = null)
        {
            return new Uri(GetBaseUriString(path, navtgt), UriKind.Relative);
        }

        private static string GetBaseUriString(string path, string navtgt)
        {
            string uri = path;
            if (navtgt != null)
            {
                uri += string.Format("?navtgt={0}", navtgt);
            }
            return uri;
        } 

        #endregion
    }
}
