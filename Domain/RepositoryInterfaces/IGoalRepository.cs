using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        Task<IEnumerable<Goal>> GetGoalsByUserAsync(int userId);
        Task<IEnumerable<Goal>> GetGoalsByWorkoutAsync(int workoutId);
        Task<IEnumerable<Goal>> GetGoalsByRoutineAsync(int routineId);
        Task<IEnumerable<Goal>> GetGoalsByExerciseAsync(int exerciseId);
    }
}
