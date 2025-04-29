namespace Application.DTOs.Routine
{
    public class RoutineOutputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public int InstructorId { get; set; }
        public int LevelId { get; set; }
        public List<int> GoalIds { get; set; } = [];
        public List<int> TypeIds { get; set; } = [];
        public List<int> ModalityIds { get; set; } = [];
        public List<int> HashtagIds { get; set; } = [];
    }
}
