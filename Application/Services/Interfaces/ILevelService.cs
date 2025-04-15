using Application.DTOs;
using Application.DTOs.Level;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Services.Interfaces;

public interface ILevelService : IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
}
