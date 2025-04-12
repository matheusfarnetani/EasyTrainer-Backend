using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IInstructorRepository : IGenericRepository<Instructor>
    {
        Task<Instructor?> GetInstructorByEmailAsync(string email);
        Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId);
        Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId);
        Task<Instructor?> GetInstructorByRoutineIdAsync(int routineId);
        Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId);
    }
}
