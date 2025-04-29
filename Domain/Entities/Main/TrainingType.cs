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
        public ICollection<WorkoutHasType> WorkoutTypes { get; set; } = [];
        public ICollection<RoutineHasType> RoutineTypes { get; set; } = [];
        public ICollection<ExerciseHasType> ExerciseTypes { get; set; } = [];
    }
}
