namespace Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}