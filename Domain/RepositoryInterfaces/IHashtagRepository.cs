using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IHashtagRepository : IGenericRepository<Hashtag>
    {
        Task<IEnumerable<Hashtag>> GetHashtagsByWorkoutAsync(int workoutId);
        Task<IEnumerable<Hashtag>> GetHashtagsByRoutineAsync(int routineId);
        Task<IEnumerable<Hashtag>> GetHashtagsByExerciseAsync(int exerciseId);
        Task<bool> ExistsByIdAsync(int id);
    }
}
