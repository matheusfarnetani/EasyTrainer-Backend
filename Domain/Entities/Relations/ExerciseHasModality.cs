using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class ExerciseHasModality
    {
        public int ExerciseId { get; set; }
        public int ModalityId { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public Modality Modality { get; set; } = null!;
    }
}
