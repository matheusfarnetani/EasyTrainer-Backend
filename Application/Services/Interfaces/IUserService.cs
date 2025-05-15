using Application.DTOs.User;
using Application.DTOs.Workout;
using Application.DTOs;
using Application.DTOs.Goal;
using Application.DTOs.Instructor;

namespace Application.Services.Interfaces
{
    public interface IUserService : IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>
    {
        Task<ServiceResponseDTO<UserOutputDTO>> GetByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(int userId, string currentPassword, string newPassword);

        // Instructor
        Task<ServiceResponseDTO<bool>> AddInstructorToUserAsync(int userId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveInstructorFromUserAsync(int userId, int instructorId);
        Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetInstructorsByUserIdAsync(int userId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);

        // Level
        Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination);

        // Goal
        Task<ServiceResponseDTO<bool>> AddGoalToUserAsync(int userId, int goalId);
        Task<ServiceResponseDTO<bool>> RemoveGoalFromUserAsync(int userId, int goalId);
        Task<ServiceResponseDTO<PaginationResponseDTO<GoalOutputDTO>>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination);

        // Workout
        Task<ServiceResponseDTO<bool>> AddWorkoutToUserAsync(int userId, int workoutId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveWorkoutFromUserAsync(int userId, int workoutId, int instructorId);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination);
    }
}