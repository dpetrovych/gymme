﻿using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoSet : RepositoryBase<Set>
    {
        private static RepoSet _instance;

        public static RepoSet Instance { get { return _instance ?? (_instance = new RepoSet()); } }

        private RepoSet()
        {
        }

        public override Set FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }

        public IEnumerable<Set> FindAllForTraining(TrainingExercise trainingExercise)
        {
            return Table.Where(x => x.IdTrainingExercise == trainingExercise.Id);
        }

        public float FindMaxWeight(TrainingExercise trainingExercise)
        {
            var sets = FindAllForTraining(trainingExercise).ToArray();
            if (!sets.Any())
            {
                return 0;
            }

            return sets.Max(x => x.Lift);
        }

        public float FindAvgWeight(TrainingExercise trainingExercise)
        {
            var sets = FindAllForTraining(trainingExercise).ToArray();
            if (!sets.Any())
            {
                return 0;
            }

            return sets.Sum(x => x.Lift * x.Reps) / sets.Sum(x => x.Reps);
        }

        public float FindRepsPerSet(TrainingExercise trainingExercise)
        {
            var sets = FindAllForTraining(trainingExercise).ToArray();
            if (!sets.Any())
            {
                return 0;
            }

            return sets.Sum(x => x.Reps) / sets.Length;
        }

        public float FindTotalReps(TrainingExercise trainingExercise)
        {
            var sets = FindAllForTraining(trainingExercise).ToArray();
            if (!sets.Any())
            {
                return 0;
            }

            return sets.Sum(x => x.Reps);
        }
    }
}