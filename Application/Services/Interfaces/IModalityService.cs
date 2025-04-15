using Application.DTOs;
using Application.DTOs.Modality;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Services.Interfaces;

public interface IModalityService : IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
}
