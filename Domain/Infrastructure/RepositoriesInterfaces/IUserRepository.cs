using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId);

        // Instructor
        Task AddInstructorToUserAsync(int userId, int instructorId);
        Task RemoveInstructorFromUserAsync(int userId, int instructorId);
        Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId);
        Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId);

        // Level
        Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId);

        // Goal
        Task AddGoalToUserAsync(int userId, int goalId);
        Task RemoveGoalFromUserAsync(int userId, int goalId);
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId);
        Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId);

        // Workout
        Task AddWorkoutToUserAsync(int userId, int workoutId);
        Task RemoveWorkoutFromUserAsync(int userId, int workoutId);
        Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
        Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId);
    }
}
