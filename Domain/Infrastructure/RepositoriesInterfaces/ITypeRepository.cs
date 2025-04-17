using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface ITypeRepository : IGenericRepository<TrainingType>
    {
        Task<IEnumerable<TrainingType>> GetTypesByWorkoutAsync(int workoutId);
        Task<IEnumerable<TrainingType>> GetTypesByRoutineAsync(int routineId);
        Task<IEnumerable<TrainingType>> GetTypesByExerciseAsync(int exerciseId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
