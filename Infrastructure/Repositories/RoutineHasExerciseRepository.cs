using Domain.Entities.Relations;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoutineHasExerciseRepository : IRoutineHasExerciseRepository
    {
        private readonly AppDbContext _context;

        public RoutineHasExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RoutineHasExercise entity)
        {
            await _context.RoutineHasExercises.AddAsync(entity);
        }

        public async Task<RoutineHasExercise?> GetByIdAsync(int routineId, int exerciseId)
        {
            return await _context.RoutineHasExercises
                .Include(re => re.Routine)
                .Include(re => re.Exercise)
                .FirstOrDefaultAsync(re => re.RoutineId == routineId && re.ExerciseId == exerciseId);
        }

        public async Task UpdateAsync(RoutineHasExercise entity)
        {
            _context.RoutineHasExercises.Update(entity);
        }

        public async Task DeleteByIdAsync(int routineId, int exerciseId)
        {
            var entity = await _context.RoutineHasExercises
                .FirstOrDefaultAsync(re => re.RoutineId == routineId && re.ExerciseId == exerciseId);

            if (entity is not null)
            {
                _context.RoutineHasExercises.Remove(entity);
            }
        }

        public async Task<IEnumerable<RoutineHasExercise>> GetExercisesByRoutineIdAsync(int routineId)
        {
            return await _context.RoutineHasExercises
                .Include(re => re.Exercise)
                .Where(re => re.RoutineId == routineId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoutineHasExercise>> GetRoutinesByExerciseIdAsync(int exerciseId)
        {
            return await _context.RoutineHasExercises
                .Include(re => re.Routine)
                .Where(re => re.ExerciseId == exerciseId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
