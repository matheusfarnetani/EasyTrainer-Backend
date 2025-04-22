using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Modality;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface IModalityService : IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>
    {
        // Workout
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);

        // Routine
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);

        // Exercise
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    }
}