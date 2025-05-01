using Application.DTOs;
using Application.DTOs.Auth;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponseDTO<LoginOutputDTO>> AuthenticateAsync(LoginInputDTO dto);
        Task<ServiceResponseDTO<LoginOutputDTO>> RegisterUserBasicAsync(CreateUserRegisterDTO dto);
        Task<ServiceResponseDTO<LoginOutputDTO>> RegisterInstructorBasicAsync(CreateInstructorRegisterDTO dto);
    }
}