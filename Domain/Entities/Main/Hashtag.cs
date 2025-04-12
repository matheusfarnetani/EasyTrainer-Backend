using System;
using System.Collections.Generic;
using Domain.Entities.Relations;

namespace Domain.Entities.Main
{
    public class Hashtag
    {
        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Name { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<WorkoutHasHashtag> WorkoutHashtags { get; set; } = new List<WorkoutHasHashtag>();
        public ICollection<RoutineHasHashtag> RoutineHashtags { get; set; } = new List<RoutineHasHashtag>();
        public ICollection<ExerciseHasHashtag> ExerciseHashtags { get; set; } = new List<ExerciseHasHashtag>();
    }
}
