using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class ExerciseHasType
    {
        public int ExerciseId { get; set; }
        public int TypeId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Type Type { get; set; } = null!;
    }
}
