using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class ExerciseHasVariation
    {
        public int ExerciseId { get; set; }
        public int VariationId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Exercise Variation { get; set; } = null!;
    }
}
