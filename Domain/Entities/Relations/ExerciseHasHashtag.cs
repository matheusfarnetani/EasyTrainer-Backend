using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class ExerciseHasHashtag
    {
        public int ExerciseId { get; set; }
        public int HashtagId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Hashtag Hashtag { get; set; } = null!;
    }
}
