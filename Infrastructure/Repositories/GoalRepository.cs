using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
    {
        private readonly AppDbContext _context;

        public GoalRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserAsync(int userId)
        {
            return await _context.Goals
                .Where(g => g.UserGoals.Any(ug => ug.UserId == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetGoalsByWorkoutAsync(int workoutId)
        {
            return await _context.Goals
                .Where(g => g.WorkoutGoals.Any(wg => wg.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetGoalsByRoutineAsync(int routineId)
        {
            return await _context.Goals
                .Where(g => g.RoutineGoals.Any(rg => rg.RoutineId == routineId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Goal>> GetGoalsByExerciseAsync(int exerciseId)
        {
            return await _context.Goals
                .Where(g => g.ExerciseGoals.Any(eg => eg.ExerciseId == exerciseId))
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Goals.AnyAsync(g => g.Id == id);
        }
    }
}
