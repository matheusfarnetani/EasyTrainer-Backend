using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class RoutineHasModality
    {
        public int RoutineId { get; set; }
        public int ModalityId { get; set; }

        public Routine Routine { get; set; } = null!;
        public Modality Modality { get; set; } = null!;
    }
}
