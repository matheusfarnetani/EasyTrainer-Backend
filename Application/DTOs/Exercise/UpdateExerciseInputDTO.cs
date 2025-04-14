namespace Application.DTOs.Exercise
{
    public class UpdateExerciseInputDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Equipment { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? Repetition { get; set; }
        public int? Sets { get; set; }
        public TimeSpan? RestTime { get; set; }
        public string? BodyPart { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? Steps { get; set; }
        public string? Contraindications { get; set; }
        public string? SafetyTips { get; set; }
        public string? CommonMistakes { get; set; }
        public string? IndicatedFor { get; set; }
        public float? CaloriesBurnedEstimate { get; set; }
        public int? LevelId { get; set; }
        public List<int>? GoalIds { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<int>? ModalityIds { get; set; }
        public List<int>? HashtagIds { get; set; }
    }
}
