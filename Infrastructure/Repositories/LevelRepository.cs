using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LevelRepository(AppDbContext context) : GenericRepository<Level>(context), ILevelRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Level?> GetLevelByUserAsync(int userId)
        {
            return await _context.Levels
                .FirstOrDefaultAsync(l => l.Users.Any(u => u.Id == userId));
        }

        public async Task<Level?> GetLevelByWorkoutAsync(int workoutId)
        {
            return await _context.Levels
                .FirstOrDefaultAsync(l => l.Workouts.Any(w => w.Id == workoutId));
        }

        public async Task<Level?> GetLevelByRoutineAsync(int routineId)
        {
            return await _context.Levels
                .FirstOrDefaultAsync(l => l.Routines.Any(r => r.Id == routineId));
        }

        public async Task<Level?> GetLevelByExerciseAsync(int exerciseId)
        {
            return await _context.Levels
                .FirstOrDefaultAsync(l => l.Exercises.Any(e => e.Id == exerciseId));
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Levels.AnyAsync(l => l.Id == id);
        }
    }
}
