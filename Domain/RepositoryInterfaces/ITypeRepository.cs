using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface ITypeRepository : IGenericRepository<TrainingType>
    {
        Task<IEnumerable<TrainingType>> GetTypesByWorkoutAsync(int workoutId);
        Task<IEnumerable<TrainingType>> GetTypesByRoutineAsync(int routineId);
        Task<IEnumerable<TrainingType>> GetTypesByExerciseAsync(int exerciseId);
    }
}
