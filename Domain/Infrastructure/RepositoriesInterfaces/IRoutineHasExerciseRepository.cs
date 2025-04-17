using Domain.Entities.Relations;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface IRoutineHasExerciseRepository
    {
        Task AddAsync(RoutineHasExercise entity);
        Task<RoutineHasExercise?> GetByIdAsync(int routineId, int exerciseId);
        Task UpdateAsync(RoutineHasExercise entity);
        Task DeleteByIdAsync(int routineId, int exerciseId);

        Task<IEnumerable<RoutineHasExercise>> GetExercisesByRoutineIdAsync(int routineId);
        Task<IEnumerable<RoutineHasExercise>> GetRoutinesByExerciseIdAsync(int exerciseId);
        Task SaveAsync();
    }
}
