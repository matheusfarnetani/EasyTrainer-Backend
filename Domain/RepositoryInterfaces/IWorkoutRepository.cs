using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IWorkoutRepository : IGenericRepository<Workout>
    {
        Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
        Task<IEnumerable<Workout>> GetWorkoutsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Workout>> GetWorkoutsByGoalIdAsync(int goalId, int? instructorId = null, int? userId = null);
        Task<IEnumerable<Workout>> GetWorkoutsByLevelIdAsync(int levelId, int? instructorId = null, int? userId = null);
        Task<IEnumerable<Workout>> GetWorkoutsByModalityIdAsync(int modalityId, int? instructorId = null, int? userId = null);
        Task<IEnumerable<Workout>> GetWorkoutsByHashtagIdAsync(int hashtagId, int? instructorId = null, int? userId = null);
        Task<IEnumerable<Workout>> GetWorkoutsByRoutineIdAsync(int routineId, int? instructorId = null, int? userId = null);
        Task<IEnumerable<Workout>> GetWorkoutsByExerciseIdAsync(int exerciseId, int? instructorId = null, int? userId = null);
    }
}
