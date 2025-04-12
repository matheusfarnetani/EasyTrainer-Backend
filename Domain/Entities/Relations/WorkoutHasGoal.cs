using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasGoal
    {
        public int WorkoutId { get; set; }
        public int GoalId { get; set; }

        public Workout Workout { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
