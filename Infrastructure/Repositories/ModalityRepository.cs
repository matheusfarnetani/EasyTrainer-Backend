using Domain.Entities.Main;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ModalityRepository : GenericRepository<Modality>, IModalityRepository
    {
        private readonly AppDbContext _context;

        public ModalityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Modality>> GetModalitiesByWorkoutAsync(int workoutId)
        {
            return await _context.Modalities
                .Where(m => m.WorkoutModalities.Any(wm => wm.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Modality>> GetModalitiesByRoutineAsync(int routineId)
        {
            return await _context.Modalities
                .Where(m => m.RoutineModalities.Any(rm => rm.RoutineId == routineId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Modality>> GetModalitiesByExerciseAsync(int exerciseId)
        {
            return await _context.Modalities
                .Where(m => m.ExerciseModalities.Any(em => em.ExerciseId == exerciseId))
                .ToListAsync();
        }
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Modalities.AnyAsync(m => m.Id == id);
        }
    }
}
