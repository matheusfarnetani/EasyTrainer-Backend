using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class TrainingType
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<WorkoutHasType> WorkoutTypes { get; set; } = new List<WorkoutHasType>();
        public ICollection<RoutineHasType> RoutineTypes { get; set; } = new List<RoutineHasType>();
        public ICollection<ExerciseHasType> ExerciseTypes { get; set; } = new List<ExerciseHasType>();
    }
}
