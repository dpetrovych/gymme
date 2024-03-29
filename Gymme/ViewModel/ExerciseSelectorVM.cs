﻿using System.Collections.ObjectModel;
using System.Linq;

using Gymme.Resources;

namespace Gymme.ViewModel
{
    public class ExerciseSelectorVM : Base.ViewModel
    {
        private readonly long _workoutId;

        public ExerciseSelectorVM(long workoutId)
        {
            _workoutId = workoutId;
            LoadExercises();
        }

        public ObservableCollection<ExerciseSelectItemVM> Items { get; private set; }

        public long WorkoutId
        {
            get { return _workoutId; }
        }

        private void LoadExercises()
        {
            if (!ExerciseData.Instance.IsDataLoaded)
            {
                ExerciseData.Instance.LoadData();
            }

            Items = new ObservableCollection<ExerciseSelectItemVM>(ExerciseData.Instance.PersetExercises.Select(x => new ExerciseSelectItemVM(x, WorkoutId)));
        }
    }
}