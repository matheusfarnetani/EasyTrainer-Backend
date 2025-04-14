using Application.DTOs;
using Application.DTOs.User;

namespace Application.Services.Interfaces
{
    public interface IUserService : IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>
    {
        Task<UserOutputDTO> GetByEmailAsync(string email);
        Task<PaginationResponseDTO<UserOutputDTO>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
        Task<PaginationResponseDTO<UserOutputDTO>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination);
        Task<PaginationResponseDTO<UserOutputDTO>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination);
        Task<PaginationResponseDTO<UserOutputDTO>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination);
    }
}
