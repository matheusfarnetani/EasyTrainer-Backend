using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class BaseRegisterDTO
    {
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
