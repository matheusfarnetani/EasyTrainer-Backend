using System.Security.Claims;
using Domain.API.Interfaces;

namespace API.Auth
{
    public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int Id => 1;
        public string Role => "admin"; // ou "instructor", "user"
        public string Email => "dev@example.com";
        //public int Id =>
        //    int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        //public string Role =>
        //    _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        //public string Email =>
        //    _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }
}