using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        // General Queries
        Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByRoutineIdAsync(int routineId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId);
        Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int exerciseId, int instructorId);

        // Direct Queries for Deletion Validation
        Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int typeId);
        Task<IEnumerable<Exercise>> GetExercisesByModalityIdAsync(int modalityId);
        Task<IEnumerable<Exercise>> GetExercisesByHashtagIdAsync(int hashtagId);
    }
}
