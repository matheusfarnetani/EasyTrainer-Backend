using Domain.Entities.Relations;

namespace Domain.RepositoryInterfaces
{
    public interface IRoutineHasExerciseRepository : IGenericRepository<RoutineHasExercise>
    {
        Task<IEnumerable<RoutineHasExercise>> GetExercisesByRoutineIdAsync(int routineId);
        Task<IEnumerable<RoutineHasExercise>> GetRoutinesByExerciseIdAsync(int exerciseId);
    }
}
