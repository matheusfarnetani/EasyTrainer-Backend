using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface IInstructorService : IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>
    {
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetByEmailAsync(string email);

        // User
        Task<ServiceResponseDTO<bool>> AddUserToInstructorAsync(int instructorId, int userId);
        Task<ServiceResponseDTO<bool>> RemoveUserFromInstructorAsync(int instructorId, int userId);
        Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);

        // Workout
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsAsync(int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetByWorkoutIdAsync(int workoutId);

        // Routine
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesAsync(int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetByRoutineIdAsync(int routineId);

        // Exercise
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesAsync(int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetByExerciseIdAsync(int exerciseId);
    }
}