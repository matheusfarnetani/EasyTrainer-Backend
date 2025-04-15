using Domain.Entities.Main;
using Domain.RepositoryInterfaces;

public interface IWorkoutRepository : IGenericRepository<Workout>
{
    Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId);

    Task AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId);
    Task RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId);

    Task AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId);
    Task RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId);

    Task AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId);
    Task RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId);

    Task AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId);
    Task RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId);

    Task AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId);
    Task RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId);

    Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId);
    Task RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId);

    Task<Workout?> GetByIdAsync(int id);
    Task DeleteByIdAsync(int id);
    Task<bool> ExistsByIdAsync(int id);

    Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
    Task<IEnumerable<Workout>> GetWorkoutsByInstructorIdAsync(int instructorId);
    Task<IEnumerable<Workout>> GetWorkoutsByGoalIdAsync(int goalId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByLevelIdAsync(int levelId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByTypeIdAsync(int typeId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByModalityIdAsync(int modalityId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByHashtagIdAsync(int hashtagId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByRoutineIdAsync(int routineId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByExerciseIdAsync(int exerciseId, int? instructorId = null, int? userId = null);
}
