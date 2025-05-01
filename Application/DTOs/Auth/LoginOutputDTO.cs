using Application.DTOs.User;
using Application.DTOs.Instructor;

namespace Application.DTOs.Auth
{
    public class LoginOutputDTO
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public UserOutputDTO? User { get; set; }
        public InstructorOutputDTO? Instructor { get; set; }
    }
}
