using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasType
    {
        public int WorkoutId { get; set; }
        public int TypeId { get; set; }

        // Navigation Properties
        public Workout Workout { get; set; } = null!;
        public TrainingType Type { get; set; } = null!;
    }
}
