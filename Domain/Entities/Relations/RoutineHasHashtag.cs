using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class RoutineHasHashtag
    {
        public int RoutineId { get; set; }
        public int HashtagId { get; set; }

        public Routine Routine { get; set; } = null!;
        public Hashtag Hashtag { get; set; } = null!;
    }
}
