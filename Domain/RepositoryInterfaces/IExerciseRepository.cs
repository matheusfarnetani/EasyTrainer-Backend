using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        // General Queries
        Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int typeId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByModalityIdAsync(int modalityId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByRoutineIdAsync(int routineId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId);
        Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int exerciseId, int instructorId);
    }
}
