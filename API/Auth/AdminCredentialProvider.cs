using Domain.API.Interfaces;

namespace API.Auth
{
    public class AdminCredentialProvider(IConfiguration config) : IAdminCredentialProvider
    {
        private readonly IConfiguration _config = config;

        public string Email => _config["AdminAuth:Email"]!;
        public string PasswordHash => _config["AdminAuth:Password"]!;
    }
}