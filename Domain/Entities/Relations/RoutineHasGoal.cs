using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class RoutineHasGoal
    {
        public int RoutineId { get; set; }
        public int GoalId { get; set; }

        public Routine Routine { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
