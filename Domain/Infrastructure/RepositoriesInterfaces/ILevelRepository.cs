using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface ILevelRepository : IGenericRepository<Level>
    {
        Task<Level?> GetLevelByUserAsync(int userId);
        Task<Level?> GetLevelByWorkoutAsync(int workoutId);
        Task<Level?> GetLevelByRoutineAsync(int routineId);
        Task<Level?> GetLevelByExerciseAsync(int exerciseId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
