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
        public ICollection<WorkoutHasHashtag> WorkoutHashtags { get; set; } = [];
        public ICollection<RoutineHasHashtag> RoutineHashtags { get; set; } = [];
        public ICollection<ExerciseHasHashtag> ExerciseHashtags { get; set; } = [];
    }
}
