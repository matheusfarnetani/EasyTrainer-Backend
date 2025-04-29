using Application.DTOs;
using Application.DTOs.RoutineHasExercise;

namespace Application.Services.Interfaces
{
    public interface IRoutineHasExerciseService
    {
        // CRUD
        Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> AddAsync(CreateRoutineHasExerciseDTO dto);
        Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> UpdateAsync(UpdateRoutineHasExerciseDTO dto);
        Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> GetByIdAsync(int routineId, int exerciseId);
        Task<ServiceResponseDTO<bool>> DeleteAsync(int routineId, int exerciseId);

        // Consultas
        Task<ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>> GetExercisesByRoutineIdAsync(int routineId);
        Task<ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>> GetRoutinesByExerciseIdAsync(int exerciseId);
    }
}
