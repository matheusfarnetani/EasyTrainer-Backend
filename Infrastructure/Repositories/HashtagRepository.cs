using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HashtagRepository(AppDbContext context) : GenericRepository<Hashtag>(context), IHashtagRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Hashtag>> GetHashtagsByWorkoutAsync(int workoutId)
        {
            return await _context.Hashtags
                .Where(h => h.WorkoutHashtags.Any(wh => wh.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Hashtag>> GetHashtagsByRoutineAsync(int routineId)
        {
            return await _context.Hashtags
                .Where(h => h.RoutineHashtags.Any(rh => rh.RoutineId == routineId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Hashtag>> GetHashtagsByExerciseAsync(int exerciseId)
        {
            return await _context.Hashtags
                .Where(h => h.ExerciseHashtags.Any(eh => eh.ExerciseId == exerciseId))
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Hashtags.AnyAsync(h => h.Id == id);
        }
    }
}
