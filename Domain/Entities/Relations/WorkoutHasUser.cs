using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasUser
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }

        public Workout Workout { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
