using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Workout
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NumberOfDays { get; set; }
        public string? ImageUrl { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Indoor { get; set; }
        public int InstructorId { get; set; }
        public int LevelId { get; set; }

        // Navigation Properties
        public Level Level { get; set; } = null!;
        public Instructor Instructor { get; set; } = null!;
        public ICollection<WorkoutHasType> WorkoutTypes { get; set; } = [];
        public ICollection<WorkoutHasModality> WorkoutModalities { get; set; } = [];
        public ICollection<WorkoutHasGoal> WorkoutGoals { get; set; } = [];
        public ICollection<WorkoutHasHashtag> WorkoutHashtags { get; set; } = [];
        public ICollection<WorkoutHasUser> WorkoutUsers { get; set; } = [];
        public ICollection<WorkoutHasRoutine> WorkoutRoutines { get; set; } = [];
        public ICollection<WorkoutHasExercise> WorkoutExercises { get; set; } = [];
    }
}
