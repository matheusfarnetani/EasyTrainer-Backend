namespace Application.DTOs.Routine
{
    public class CreateRoutineInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public int LevelId { get; set; }
        public List<int> GoalIds { get; set; } = new List<int>();
        public List<int> TypeIds { get; set; } = new List<int>();
        public List<int> ModalityIds { get; set; } = new List<int>();
        public List<int> HashtagIds { get; set; } = new List<int>();
    }
}
