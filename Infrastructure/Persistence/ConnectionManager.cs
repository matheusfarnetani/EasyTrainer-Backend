using Domain.API.Interfaces;
using Domain.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class ConnectionManager(IConfiguration configuration, ICurrentUserContext userContext) : IConnectionManager
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ICurrentUserContext _userContext = userContext;

        public string GetCurrentConnectionString()
        {
            return GetConnectionStringOrDefault("admin");
        }

        public string GetConnectionString(string role)
        {
            var connection = role switch
            {
                "admin" => _configuration.GetConnectionString("EasyTrainerAdmin"),
                "instructor" => _configuration.GetConnectionString("EasyTrainerInstructor"),
                "user" => _configuration.GetConnectionString("EasyTrainerUser"),
                _ => throw new ArgumentException("Invalid role")
            };

            return connection ?? throw new InvalidOperationException($"Connection string for role '{role}' is not configured.");
        }

        public string GetConnectionStringOrDefault(string defaultRole)
        {
            var role = _userContext.Role;
            if (string.IsNullOrWhiteSpace(role))
            {
                role = defaultRole;
            }

            return GetConnectionString(role);
        }
    }
}