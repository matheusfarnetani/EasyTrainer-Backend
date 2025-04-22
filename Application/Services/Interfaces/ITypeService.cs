using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.TrainingType;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface ITypeService : IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>
    {
        // Workout
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);

        // Routine
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);

        // Exercise
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    }
}