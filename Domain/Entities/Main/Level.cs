namespace Domain.Entities.Main
{
    public class Level
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Workout> Workouts { get; set; } = [];
        public ICollection<Routine> Routines { get; set; } = [];
        public ICollection<Exercise> Exercises { get; set; } = [];
    }
}
