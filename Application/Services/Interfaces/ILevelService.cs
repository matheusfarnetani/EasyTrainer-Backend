using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Level;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface ILevelService : IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>
    {
        // Workout
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);

        // Routine
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);

        // Exercise
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    }
}