using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.API.Interfaces;

namespace API.Auth
{
    public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int Id
        {
            get
            {
                if (IsExternalRequest)
                    return -1;

                var user = _httpContextAccessor.HttpContext?.User;
                var idClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(idClaim))
                    return 0;

                return int.Parse(idClaim);
            }
        }

        public string Role =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        public string Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        public bool IsExternalRequest =>
            string.Equals(_httpContextAccessor.HttpContext?.Request?.Headers["X-External-Request"].ToString(), "true", StringComparison.OrdinalIgnoreCase);

    }
}