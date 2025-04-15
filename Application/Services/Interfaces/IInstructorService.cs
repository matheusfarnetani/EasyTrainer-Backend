using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface IInstructorService : IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>
    {
        Task<InstructorOutputDTO> GetByEmailAsync(string email);
        Task<PaginationResponseDTO<InstructorOutputDTO>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);
        Task<InstructorOutputDTO> GetByWorkoutIdAsync(int workoutId);
        Task<InstructorOutputDTO> GetByRoutineIdAsync(int routineId);
        Task<InstructorOutputDTO> GetByExerciseIdAsync(int exerciseId);
        Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsAsync(int instructorId, PaginationRequestDTO pagination);
        Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesAsync(int instructorId, PaginationRequestDTO pagination);
        Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesAsync(int instructorId, PaginationRequestDTO pagination);

    }
}
