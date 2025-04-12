using System;
using System.Collections.Generic;
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

        // Navigation Properties
        public ICollection<UserHasInstructor> UserInstructors { get; set; } = new List<UserHasInstructor>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
