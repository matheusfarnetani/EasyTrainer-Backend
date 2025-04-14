using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IRoutineRepository : IGenericRepository<Routine>
    {
        // General Queries
        Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByExerciseIdAsync(int exerciseId, int instructorId);

        // Direct Queries for Deletion Validation
        Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId);
        Task<IEnumerable<Routine>> GetRoutinesByModalityIdAsync(int modalityId);
        Task<IEnumerable<Routine>> GetRoutinesByHashtagIdAsync(int hashtagId);
    }
}
