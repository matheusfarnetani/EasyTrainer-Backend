using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Exercise
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Equipment { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }
        public int? Repetition { get; set; }
        public int? Sets { get; set; }
        public TimeSpan? RestTime { get; set; }
        public string? BodyPart { get; set; } = string.Empty;
        public string? VideoUrl { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Steps { get; set; } = string.Empty;
        public string? Contraindications { get; set; } = string.Empty;
        public string? SafetyTips { get; set; } = string.Empty;
        public string? CommonMistakes { get; set; } = string.Empty;
        public string? IndicatedFor { get; set; } = string.Empty;
        public float? CaloriesBurnedEstimate { get; set; }
        public int InstructorId { get; set; }
        public int LevelId { get; set; }

        // Navigation Properties
        public Instructor Instructor { get; set; } = null!;
        public Level Level { get; set; } = null!;
        public ICollection<ExerciseHasType> ExerciseTypes { get; set; } = [];
        public ICollection<ExerciseHasModality> ExerciseModalities { get; set; } = [];
        public ICollection<ExerciseHasGoal> ExerciseGoals { get; set; } = [];
        public ICollection<ExerciseHasHashtag> ExerciseHashtags { get; set; } = [];
        public ICollection<ExerciseHasVariation> ExerciseVariations { get; set; } = [];
        public ICollection<RoutineHasExercise> RoutineExercises { get; set; } = [];
        public ICollection<WorkoutHasExercise> WorkoutExercises { get; set; } = [];
        public ICollection<ExerciseHasVariation> IsVariationOf { get; set; } = [];
    }
}
