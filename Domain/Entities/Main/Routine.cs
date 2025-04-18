using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Routine
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public int LevelId { get; set; }

        // Navigation Properties
        public Instructor Instructor { get; set; } = null!;
        public Level Level { get; set; } = null!;
        public ICollection<RoutineHasType> RoutineTypes { get; set; } = new List<RoutineHasType>();
        public ICollection<RoutineHasModality> RoutineModalities { get; set; } = new List<RoutineHasModality>();
        public ICollection<RoutineHasGoal> RoutineGoals { get; set; } = new List<RoutineHasGoal>();
        public ICollection<RoutineHasHashtag> RoutineHashtags { get; set; } = new List<RoutineHasHashtag>();
        public ICollection<RoutineHasExercise> RoutineExercises { get; set; } = new List<RoutineHasExercise>();
        public ICollection<WorkoutHasRoutine> WorkoutRoutines { get; set; } = new List<WorkoutHasRoutine>();
    }
}
