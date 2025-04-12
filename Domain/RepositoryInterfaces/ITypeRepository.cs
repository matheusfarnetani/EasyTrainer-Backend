using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface ITypeRepository : IGenericRepository<Type>
    {
        Task<IEnumerable<Type>> GetTypesByWorkoutAsync(int workoutId);
        Task<IEnumerable<Type>> GetTypesByRoutineAsync(int routineId);
        Task<IEnumerable<Type>> GetTypesByExerciseAsync(int exerciseId);
    }
}
