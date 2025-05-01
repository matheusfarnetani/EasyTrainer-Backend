namespace Application.DTOs.Auth
{
    public class LoginInputDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
