﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

using Gymme.Data.Core;
using Gymme.Data.Models;
using Gymme.Data.Repository;

namespace Gymme.ViewModel.Page
{
    public class TrainingPageVM : Base.ViewModel, IDisposable
    {
        private readonly DispatcherTimer _timer;

        private Training _training;
        private Workout _workout;
        private TimeSpan _time;

        public TrainingPageVM(Workout workout)
            : this()
        {
            var training = new Training(workout);
            RepoTraining.Instance.Save(training);

            foreach (var exercise in workout.Exercises.OrderBy(x => x.Order))
            {
                var executeEx = new TrainingExercise(exercise.Id) { IdTraining = training.Id };
                DatabaseContext.Instance.TrainingExercise.InsertOnSubmit(executeEx);
                training.Exercises.Add(executeEx);
            }

            DatabaseContext.Instance.SubmitChanges();
            Initialize(training);
        }

        public TrainingPageVM(Training training)
            : this()
        {
            Initialize(training);
        }

        private TrainingPageVM()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.04) };
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs args)
        {
            Time = _training != null ? (DateTime.Now - _training.StartTime).TrimToSeconds() : TimeSpan.Zero;
        }

        private void Initialize(Training training)
        {
            _training = training;
            _workout = RepoWorkout.Instance.FindById(Training.IdWorkout);

            Exercises = new ObservableCollection<TrainingExerciseVM>();
            Update();
        }

        public int BackCount { get; set; }

        public string Title { get { return _workout.Title; } }

        public TimeSpan Time
        {
            get { return _time; }
            set 
            {
                if (_time != value)
                {
                    _time = value;
                    NotifyPropertyChanged("Time"); 
                }
            }
        }

        public ObservableCollection<TrainingExerciseVM> Exercises { get; private set; }

        public Training Training
        {
            get { return _training; }
        }

        public void Update()
        {
            Exercises.Clear();
            Training.Exercises.Load();
            foreach (var trainingExercise in Training.Exercises)
            {
                var exercise = RepoExercise.Instance.FindById(trainingExercise.IdExecise);
                if (exercise != null)
                {
                    Exercises.Add(new TrainingExerciseVM(trainingExercise, exercise, () => NotifyPropertyChanged("Exercises")));
                }
            }
        }

        public void Finish()
        {
            Training.Status = TrainingStatus.Finished;
            RepoTraining.Instance.Save(Training);
        }

        public bool Delete()
        {
            MessageBoxResult result = MessageBox.Show(Resources.AppResources.Training_DeleteWarning,
                                                      Resources.AppResources.Training_DeleteWarningTitle,
                                                      MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                RepoTraining.Instance.Delete(Training);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _timer.Stop();
        }
    }
}