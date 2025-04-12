using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasHashtag
    {
        public int WorkoutId { get; set; }
        public int HashtagId { get; set; }

        public Workout Workout { get; set; } = null!;
        public Hashtag Hashtag { get; set; } = null!;
    }
}
