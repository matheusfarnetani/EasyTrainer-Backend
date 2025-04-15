using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int typeId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByModalityIdAsync(int modalityId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByRoutineIdAsync(int routineId, int instructorId);
        Task<IEnumerable<Exercise>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId);
        Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int exerciseId, int instructorId);

        Task<Exercise?> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);

        Task AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId);
        Task RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId);

        Task AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId);
        Task RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId);

        Task AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId);
        Task RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId);

        Task AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId);
        Task RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId);

        Task AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId);
        Task RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId);

        Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId);
    }
}
