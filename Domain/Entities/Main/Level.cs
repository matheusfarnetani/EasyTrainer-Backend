using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

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
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
