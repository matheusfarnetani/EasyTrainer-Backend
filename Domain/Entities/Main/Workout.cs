using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Workout
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfDays { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public bool Indoor { get; set; }
        public int InstructorId { get; set; }
        public int LevelId { get; set; }

        // Navigation Properties
        public ICollection<WorkoutHasType> WorkoutTypes { get; set; } = new List<WorkoutHasType>();
        public ICollection<WorkoutHasModality> WorkoutModalities { get; set; } = new List<WorkoutHasModality>();
        public ICollection<WorkoutHasGoal> WorkoutGoals { get; set; } = new List<WorkoutHasGoal>();
        public ICollection<WorkoutHasHashtag> WorkoutHashtags { get; set; } = new List<WorkoutHasHashtag>();
        public ICollection<WorkoutHasUser> WorkoutUsers { get; set; } = new List<WorkoutHasUser>();
        public ICollection<WorkoutHasRoutine> WorkoutRoutines { get; set; } = new List<WorkoutHasRoutine>();
        public ICollection<WorkoutHasExercise> WorkoutExercises { get; set; } = new List<WorkoutHasExercise>();
    }
}
