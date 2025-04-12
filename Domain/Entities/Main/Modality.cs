using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Modality
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<WorkoutHasModality> WorkoutModalities { get; set; } = new List<WorkoutHasModality>();
        public ICollection<RoutineHasModality> RoutineModalities { get; set; } = new List<RoutineHasModality>();
        public ICollection<ExerciseHasModality> ExerciseModalities { get; set; } = new List<ExerciseHasModality>();
    }
}
