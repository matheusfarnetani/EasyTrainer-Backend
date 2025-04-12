using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class UserHasGoal
    {
        // Composite Key (UserId + GoalId)
        public int UserId { get; set; }
        public int GoalId { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
        public Goal Goal { get; set; } = null!;
    }
}
