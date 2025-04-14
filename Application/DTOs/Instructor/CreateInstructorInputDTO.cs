namespace Application.DTOs.Instructor
{
    public class CreateInstructorInputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public char Gender { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
