namespace Application.DTOs.User
{
    public class CreateUserInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public char Gender { get; set; }
        public string Password { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public List<int> GoalIds { get; set; } = new List<int>();
        public List<int> InstructorIds { get; set; } = new List<int>();
    }
}
