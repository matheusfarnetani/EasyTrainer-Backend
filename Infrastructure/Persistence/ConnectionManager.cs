using Domain.API.Interfaces;
using Domain.Application.Interfaces;
using Domain.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserContext _userContext;

        public ConnectionManager(IConfiguration configuration, ICurrentUserContext userContext)
        {
            _configuration = configuration;
            _userContext = userContext;
        }

        public string GetCurrentConnectionString()
        {
            var role = _userContext.GetCurrentRole();
            return GetConnectionString(role);
        }

        public string GetConnectionString(string role)
        {
            return role switch
            {
                "admin" => _configuration.GetConnectionString("EasyTrainerAdmin"),
                "instructor" => _configuration.GetConnectionString("EasyTrainerInstructor"),
                "user" => _configuration.GetConnectionString("EasyTrainerUser"),
                _ => throw new ArgumentException("Invalid role")
            };
        }
    }
}