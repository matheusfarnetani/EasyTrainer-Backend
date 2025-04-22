using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface IRoutineRepository : IGenericRepository<Routine>
    {
        Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByExerciseIdAsync(int exerciseId, int instructorId);
        Task<Instructor?> GetInstructorByRoutineIdAsync(int routineId);

        Task AddGoalToRoutineAsync(int routineId, int goalId, int instructorId);
        Task RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId);

        Task AddTypeToRoutineAsync(int routineId, int typeId, int instructorId);
        Task RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId);

        Task AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId);
        Task RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId);

        Task AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId);
        Task RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId);

        Task AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId);
        Task RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId);
    }
}
