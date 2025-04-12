using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasModality
    {
        public int WorkoutId { get; set; }
        public int ModalityId { get; set; }

        public Workout Workout { get; set; } = null!;
        public Modality Modality { get; set; } = null!;
    }
}
