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
        public ICollection<WorkoutHasModality> WorkoutModalities { get; set; } = [];
        public ICollection<RoutineHasModality> RoutineModalities { get; set; } = [];
        public ICollection<ExerciseHasModality> ExerciseModalities { get; set; } = [];
    }
}
