namespace Application.DTOs.Exercise
{
    public class CreateExerciseInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int Repetition { get; set; }
        public int Sets { get; set; }
        public TimeSpan RestTime { get; set; }
        public string BodyPart { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Steps { get; set; } = string.Empty;
        public string Contraindications { get; set; } = string.Empty;
        public string SafetyTips { get; set; } = string.Empty;
        public string CommonMistakes { get; set; } = string.Empty;
        public string IndicatedFor { get; set; } = string.Empty;
        public float CaloriesBurnedEstimate { get; set; }
        public int InstructorId { get; set; }
        public int LevelId { get; set; }
        public List<int> GoalIds { get; set; } = [];
        public List<int> TypeIds { get; set; } = [];
        public List<int> ModalityIds { get; set; } = [];
        public List<int> HashtagIds { get; set; } = [];
    }
}
