namespace Application.DTOs.User
{
    public class UpdateUserInputDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public char? Gender { get; set; }
        public string? Password { get; set; }
        public int? LevelId { get; set; }
        public List<int>? GoalIds { get; set; }
        public List<int>? InstructorIds { get; set; }
    }
}
