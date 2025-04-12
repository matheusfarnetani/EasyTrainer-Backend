﻿using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Goal
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<UserHasGoal> UserGoals { get; set; } = new List<UserHasGoal>();
        public ICollection<WorkoutHasGoal> WorkoutGoals { get; set; } = new List<WorkoutHasGoal>();
        public ICollection<RoutineHasGoal> RoutineGoals { get; set; } = new List<RoutineHasGoal>();
        public ICollection<ExerciseHasGoal> ExerciseGoals { get; set; } = new List<ExerciseHasGoal>();
    }
}
