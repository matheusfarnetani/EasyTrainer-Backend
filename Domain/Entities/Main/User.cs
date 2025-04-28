using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class User
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public char Gender { get; set; }
        public string Password { get; set; } = string.Empty;

        // Computed Property
        public int Age => DateTime.Today.Year - Birthday.Year - (Birthday.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - Birthday.Year)) ? 1 : 0);

        // Foreign Key
        public int LevelId { get; set; }
        public int InstructorId { get; set; }

        // Navigation
        public Level Level { get; set; } = null!;
        public Instructor Instructor { get; set; } = null!;

        // Many-to-Many via junction tables
        public ICollection<UserHasInstructor> UserInstructors { get; set; } = new List<UserHasInstructor>();
        public ICollection<UserHasGoal> UserGoals { get; set; } = new List<UserHasGoal>();
        public ICollection<WorkoutHasUser> Workouts { get; set; } = new List<WorkoutHasUser>();
        public ICollection<WorkoutHasUser> WorkoutUsers { get; set; } = new List<WorkoutHasUser>();
    }
}
