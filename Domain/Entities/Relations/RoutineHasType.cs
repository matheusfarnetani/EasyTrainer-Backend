using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class RoutineHasType
    {
        public int RoutineId { get; set; }
        public int TypeId { get; set; }

        public Routine Routine { get; set; } = null!;
        public TrainingType Type { get; set; } = null!;
    }
}
