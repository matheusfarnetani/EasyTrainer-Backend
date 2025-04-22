using Domain.Entities.Main;
using Domain.Entities.Relations;
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

        // Instructor
        public async Task AddInstructorToUserAsync(int userId, int instructorId)
        {
            _context.Add(new UserHasInstructor { UserId = userId, InstructorId = instructorId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveInstructorFromUserAsync(int userId, int instructorId)
        {
            var entity = await _context.UserHasInstructors
                .FirstOrDefaultAsync(x => x.UserId == userId && x.InstructorId == instructorId);

            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId)
        {
            return await _context.UserHasInstructors
                .Where(x => x.UserId == userId)
                .Include(x => x.Instructor)
                .Select(x => x.Instructor)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId)
        {
            return await _context.Users
                .Where(u => u.UserInstructors.Any(i => i.InstructorId == instructorId))
                .ToListAsync();
        }

        // Level
        public async Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId)
        {
            return await _context.Users
                .Where(u => u.LevelId == levelId)
                .ToListAsync();
        }

        // Goal
        public async Task AddGoalToUserAsync(int userId, int goalId)
        {
            _context.Add(new UserHasGoal { UserId = userId, GoalId = goalId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGoalFromUserAsync(int userId, int goalId)
        {
            var entity = await _context.UserHasGoals
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GoalId == goalId);

            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _context.UserHasGoals
                .Where(x => x.UserId == userId)
                .Include(x => x.Goal)
                .Select(x => x.Goal)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId)
        {
            return await _context.Users
                .Where(u => u.UserGoals.Any(g => g.GoalId == goalId))
                .ToListAsync();
        }

        // Workout
        public async Task AddWorkoutToUserAsync(int userId, int workoutId)
        {
            _context.Add(new WorkoutHasUser
            {
                UserId = userId,
                WorkoutId = workoutId
            });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveWorkoutFromUserAsync(int userId, int workoutId)
        {
            var entity = await _context.WorkoutHasUsers
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.WorkoutId == workoutId);

            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId)
        {
            return await _context.WorkoutHasUsers
                .Where(x => x.UserId == userId)
                .Include(x => x.Workout)
                .Select(x => x.Workout)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId)
        {
            return await _context.Users
                .Where(u => u.WorkoutUsers.Any(w => w.WorkoutId == workoutId))
                .ToListAsync();
        }

    }
}
