using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface IRoutineRepository : IGenericRepository<Routine>
    {
        Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByModalityAsync(int modalityId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByHashtagAsync(int hashtagId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByWorkoutAsync(int workoutId, int instructorId);
        Task<IEnumerable<Routine>> GetRoutinesByExerciseAsync(int exerciseId, int instructorId);
    }
}
