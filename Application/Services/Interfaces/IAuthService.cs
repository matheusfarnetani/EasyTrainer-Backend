using Application.DTOs;
using Application.DTOs.Auth;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponseDTO<AuthResponseDTO>> AuthenticateAsync(LoginRequestDTO loginDto);
    }
}