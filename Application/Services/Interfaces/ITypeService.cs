using Application.DTOs;
using Application.DTOs.TrainingType;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Services.Interfaces;

public interface ITypeService : IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
}
