using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasRoutine
    {
        public int WorkoutId { get; set; }
        public int RoutineId { get; set; }

        public Workout Workout { get; set; } = null!;
        public Routine Routine { get; set; } = null!;
    }
}
