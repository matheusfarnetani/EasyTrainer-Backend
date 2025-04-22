using Domain.API.Interfaces;

namespace API.Auth
{
    public class AdminCredentialProvider : IAdminCredentialProvider
    {
        private readonly IConfiguration _config;

        public AdminCredentialProvider(IConfiguration config)
        {
            _config = config;
        }

        public string Email => _config["AdminAuth:Email"]!;
        public string PasswordHash => _config["AdminAuth:Password"]!;
    }
}