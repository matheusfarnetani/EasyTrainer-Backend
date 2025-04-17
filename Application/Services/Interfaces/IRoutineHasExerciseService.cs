using Application.DTOs.RoutineHasExercise;

namespace Application.Services.Interfaces
{
    public interface IRoutineHasExerciseService
    {
        Task<IEnumerable<RoutineHasExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId);
        Task<IEnumerable<RoutineHasExerciseOutputDTO>> GetRoutinesByExerciseIdAsync(int exerciseId);

        Task<RoutineHasExerciseOutputDTO> AddAsync(CreateRoutineHasExerciseDTO dto);
        Task<RoutineHasExerciseOutputDTO> UpdateAsync(UpdateRoutineHasExerciseDTO dto);
        Task DeleteAsync(int routineId, int exerciseId);
    }
}
