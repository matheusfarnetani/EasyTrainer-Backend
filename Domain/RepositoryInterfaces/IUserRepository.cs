using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId);
        Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId);
        Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId);
        Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId);
    }
}
