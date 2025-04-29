using Application.DTOs;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Domain.API.Interfaces;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IAdminCredentialProvider _adminCredentials;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IAdminCredentialProvider adminCredentials)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _adminCredentials = adminCredentials;
        }

        public async Task<ServiceResponseDTO<AuthResponseDTO>> AuthenticateAsync(LoginRequestDTO dto)
        {
            object? entity = null;
            string normalizedRole = dto.Role.ToLower();

            switch (normalizedRole)
            {
                case "user":
                    entity = await _unitOfWork.Users.GetUserByEmailAsync(dto.Email);
                    break;
                case "instructor":
                    entity = await _unitOfWork.Instructors.GetInstructorByEmailAsync(dto.Email);
                    break;
                case "admin":
                    if (dto.Email.ToLower() != _adminCredentials.Email.ToLower() ||
                        !BCrypt.Net.BCrypt.Verify(dto.Password, _adminCredentials.PasswordHash))
                    {
                        return ServiceResponseDTO<AuthResponseDTO>.CreateFailure("Invalid credentials.");
                    }

                    entity = new { Id = 0, Email = dto.Email };
                    break;
                default:
                    return ServiceResponseDTO<AuthResponseDTO>.CreateFailure("Invalid role specified.");
            }

            if (entity == null)
                return ServiceResponseDTO<AuthResponseDTO>.CreateFailure("Account not found.");

            var password = entity.GetType().GetProperty("Password")?.GetValue(entity)?.ToString();
            var id = (int?)entity.GetType().GetProperty("Id")?.GetValue(entity);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, password) || id == null)
                return ServiceResponseDTO<AuthResponseDTO>.CreateFailure("Invalid credentials.");

            var token = _jwtService.GenerateToken(id.Value, normalizedRole, dto.Email);

            return ServiceResponseDTO<AuthResponseDTO>.CreateSuccess(new AuthResponseDTO
            {
                Token = token,
                Id = id.Value,
                Role = normalizedRole
            });
        }

    }

}
