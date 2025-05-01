using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Instructor
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public char? Gender { get; set; }
        public string Password { get; set; } = string.Empty;

        // Computed Property
        public int Age => DateTime.Today.Year - Birthday.Year - (Birthday.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - Birthday.Year)) ? 1 : 0);

        // Navigation Properties
        public ICollection<UserHasInstructor> UserInstructors { get; set; } = [];
        public ICollection<Workout> Workouts { get; set; } = [];
        public ICollection<Routine> Routines { get; set; } = [];
        public ICollection<Exercise> Exercises { get; set; } = [];
    }
}
