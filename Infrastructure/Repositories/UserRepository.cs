using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        private readonly AppDbContext _context = context;

        public new async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Level)
                .Include(u => u.Instructor)
                .Include(u => u.UserGoals)
                    .ThenInclude(ug => ug.Goal)
                .Include(u => u.UserInstructors)
                    .ThenInclude(ui => ui.Instructor)
                .Include(u => u.Workouts)
                    .ThenInclude(wu => wu.Workout)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Level)
                .Include(u => u.Instructor)
                .Include(u => u.UserGoals)
                    .ThenInclude(g => g.Goal)
                .Include(u => u.UserInstructors)
                    .ThenInclude(i => i.Instructor)
                .Include(u => u.Workouts)
                    .ThenInclude(wu => wu.Workout)
                .FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && u.Id != currentUserId);
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
                .Include(u => u.Level)
                .Include(u => u.Instructor)
                .Include(u => u.UserGoals)
                    .ThenInclude(g => g.Goal)
                .Include(u => u.UserInstructors)
                    .ThenInclude(i => i.Instructor)
                .Include(u => u.Workouts)
                    .ThenInclude(wu => wu.Workout)
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
                .Include(u => u.Level)
                .Include(u => u.Instructor)
                .Include(u => u.UserGoals)
                    .ThenInclude(g => g.Goal)
                .Include(u => u.UserInstructors)
                    .ThenInclude(i => i.Instructor)
                .Include(u => u.Workouts)
                    .ThenInclude(wu => wu.Workout)
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
                    .ThenInclude(w => w.WorkoutGoals)
                        .ThenInclude(g => g.Goal)
                .Include(x => x.Workout)
                    .ThenInclude(w => w.WorkoutHashtags)
                        .ThenInclude(h => h.Hashtag)
                .Include(x => x.Workout)
                    .ThenInclude(w => w.WorkoutModalities)
                        .ThenInclude(m => m.Modality)
                .Include(x => x.Workout)
                    .ThenInclude(w => w.WorkoutTypes)
                        .ThenInclude(t => t.Type)
                .Select(x => x.Workout)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId)
        {
            return await _context.Users
                .Where(u => u.Workouts.Any(w => w.WorkoutId == workoutId))
                .Include(u => u.Level)
                .Include(u => u.Instructor)
                .Include(u => u.UserGoals)
                    .ThenInclude(g => g.Goal)
                .Include(u => u.UserInstructors)
                    .ThenInclude(i => i.Instructor)
                .Include(u => u.Workouts)
                    .ThenInclude(wu => wu.Workout)
                .ToListAsync();
        }
    }
}
