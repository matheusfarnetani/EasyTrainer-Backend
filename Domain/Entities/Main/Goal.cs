using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Goal
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<UserHasGoal> UserGoals { get; set; } = [];
        public ICollection<WorkoutHasGoal> WorkoutGoals { get; set; } = [];
        public ICollection<RoutineHasGoal> RoutineGoals { get; set; } = [];
        public ICollection<ExerciseHasGoal> ExerciseGoals { get; set; } = [];
    }
}
