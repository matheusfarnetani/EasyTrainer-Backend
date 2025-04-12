using System;
using System.Collections.Generic;
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
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Birthday.Year;

                if (Birthday.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        // Foreign Keys
        public int LevelId { get; set; }
        public int InstructorId { get; set; }

        // Navigation Properties
        public ICollection<UserHasInstructor> UserInstructors { get; set; } = new List<UserHasInstructor>();
        public ICollection<UserHasGoal> UserGoals { get; set; } = new List<UserHasGoal>();
        public ICollection<WorkoutHasUser> UsersWorkout { get; set; } = new List<WorkoutHasUser>();
    }
}
