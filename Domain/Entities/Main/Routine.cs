using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Routine
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public int LevelId { get; set; }

        // Navigation Properties
        public Instructor Instructor { get; set; } = null!;
        public Level Level { get; set; } = null!;
        public ICollection<RoutineHasType> RoutineTypes { get; set; } = [];
        public ICollection<RoutineHasModality> RoutineModalities { get; set; } = [];
        public ICollection<RoutineHasGoal> RoutineGoals { get; set; } = [];
        public ICollection<RoutineHasHashtag> RoutineHashtags { get; set; } = [];
        public ICollection<RoutineHasExercise> RoutineExercises { get; set; } = [];
        public ICollection<WorkoutHasRoutine> WorkoutRoutines { get; set; } = [];
    }
}
