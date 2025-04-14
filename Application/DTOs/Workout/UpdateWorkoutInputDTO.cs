namespace Application.DTOs.Workout
{
    public class UpdateWorkoutInputDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? NumberOfDays { get; set; }
        public string? ImageUrl { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? Indoor { get; set; }
        public int? LevelId { get; set; }
        public List<int>? GoalIds { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<int>? ModalityIds { get; set; }
        public List<int>? HashtagIds { get; set; }
    }
}
