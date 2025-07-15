using Application.DTOs;
using Application.DTOs.Auth;
using Application.DTOs.Instructor;
using Application.DTOs.User;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.API.Interfaces;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IAdminCredentialProvider adminCredentials,
        IMapper mapper) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IAdminCredentialProvider _adminCredentials = adminCredentials;
        private readonly IMapper _mapper = mapper;

        public async Task<ServiceResponseDTO<LoginOutputDTO>> AuthenticateAsync(LoginInputDTO dto)
        {
            object? entity;
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
                    if (!dto.Email.Equals(_adminCredentials.Email, StringComparison.CurrentCultureIgnoreCase) ||
                        !BCrypt.Net.BCrypt.Verify(dto.Password, _adminCredentials.PasswordHash))
                    {
                        return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Invalid credentials.");
                    }

                    entity = new { Id = 0, dto.Email };
                    break;
                default:
                    return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Invalid role specified.");
            }

            if (entity == null)
                return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Account not found.");

            var password = entity.GetType().GetProperty("Password")?.GetValue(entity)?.ToString();
            var id = (int?)entity.GetType().GetProperty("Id")?.GetValue(entity);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, password) || id == null)
                return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Invalid credentials.");

            var token = _jwtService.GenerateToken(id.Value, normalizedRole, dto.Email);

            return ServiceResponseDTO<LoginOutputDTO>.CreateSuccess(new LoginOutputDTO
            {
                Token = token,
                Role = normalizedRole,
                User = normalizedRole == "user" ? _mapper.Map<UserOutputDTO>(entity) : null,
                Instructor = normalizedRole == "instructor" ? _mapper.Map<InstructorOutputDTO>(entity) : null
            });
        }

        public async Task<ServiceResponseDTO<LoginOutputDTO>> RegisterUserBasicAsync(CreateUserRegisterDTO dto)
        {
            if (await _unitOfWork.Users.ExistsByEmailAsync(dto.Email))
                return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Email already in use.");

            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Gender = null;
            user.LevelId = null;
            user.InstructorId = null;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveAsync();

            var token = _jwtService.GenerateToken(user.Id, "user", user.Email);

            return ServiceResponseDTO<LoginOutputDTO>.CreateSuccess(new LoginOutputDTO
            {
                Token = token,
                Role = "user",
                User = _mapper.Map<UserOutputDTO>(user)
            });
        }

        public async Task<ServiceResponseDTO<LoginOutputDTO>> RegisterInstructorBasicAsync(CreateInstructorRegisterDTO dto)
        {
            if (await _unitOfWork.Instructors.ExistsByEmailAsync(dto.Email))
                return ServiceResponseDTO<LoginOutputDTO>.CreateFailure("Email already in use.");

            var instructor = _mapper.Map<Instructor>(dto);
            instructor.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            instructor.Gender = null;

            await _unitOfWork.Instructors.AddAsync(instructor);
            await _unitOfWork.SaveAsync();

            var token = _jwtService.GenerateToken(instructor.Id, "instructor", instructor.Email);

            return ServiceResponseDTO<LoginOutputDTO>.CreateSuccess(new LoginOutputDTO
            {
                Token = token,
                Role = "instructor",
                Instructor = _mapper.Map<InstructorOutputDTO>(instructor)
            });
        }
    }

}
