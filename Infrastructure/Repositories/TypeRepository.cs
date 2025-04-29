using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TypeRepository(AppDbContext context) : GenericRepository<TrainingType>(context), ITypeRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<TrainingType>> GetTypesByWorkoutAsync(int workoutId)
        {
            return await _context.Types
                .Where(t => t.WorkoutTypes.Any(wt => wt.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainingType>> GetTypesByRoutineAsync(int routineId)
        {
            return await _context.Types
                .Where(t => t.RoutineTypes.Any(rt => rt.RoutineId == routineId))
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainingType>> GetTypesByExerciseAsync(int exerciseId)
        {
            return await _context.Types
                .Where(t => t.ExerciseTypes.Any(et => et.ExerciseId == exerciseId))
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Types.AnyAsync(t => t.Id == id);
        }
    }
}