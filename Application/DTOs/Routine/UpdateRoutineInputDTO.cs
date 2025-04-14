namespace Application.DTOs.Routine
{
    public class UpdateRoutineInputDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? ImageUrl { get; set; }
        public int? LevelId { get; set; }
        public List<int>? GoalIds { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<int>? ModalityIds { get; set; }
        public List<int>? HashtagIds { get; set; }
    }
}
