using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class ExerciseHasGoal
    {
        public int ExerciseId { get; set; }
        public int GoalId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
