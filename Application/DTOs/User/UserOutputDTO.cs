namespace Application.DTOs.User
{
    public class UserOutputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public char Gender { get; set; }
        public int LevelId { get; set; }
        public List<int> GoalIds { get; set; } = [];
        public List<int> InstructorIds { get; set; } = [];
    }
}
