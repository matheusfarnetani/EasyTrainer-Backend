using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class RoutineHasExercise
    {
        public int RoutineId { get; set; }
        public int ExerciseId { get; set; }

        // Additional Attributes
        public int? Order { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public TimeSpan? RestTime { get; set; }
        public string? Note { get; set; } = string.Empty;
        public int? Day { get; set; }
        public int? Week { get; set; }
        public bool? IsOptional { get; set; }

        // Navigation Properties
        public Routine Routine { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
