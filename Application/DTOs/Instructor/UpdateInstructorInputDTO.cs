namespace Application.DTOs.Instructor
{
    public class UpdateInstructorInputDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public char? Gender { get; set; }
        public string? Password { get; set; }
    }
}
