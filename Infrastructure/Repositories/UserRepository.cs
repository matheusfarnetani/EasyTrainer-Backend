using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId)
        {
            return await _context.Users
                .Where(u => u.UserInstructors.Any(i => i.InstructorId == instructorId))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId)
        {
            return await _context.Users
                .Where(u => u.UserGoals.Any(g => g.GoalId == goalId))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId)
        {
            return await _context.Users
                .Where(u => u.LevelId == levelId)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId)
        {
            return await _context.Users
                .Where(u => u.WorkoutUsers.Any(w => w.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != currentUserId);
        }
    }
}
