using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoutineRepository : GenericRepository<Routine>, IRoutineRepository
    {
        private readonly AppDbContext _context;

        public RoutineRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId)
        {
            return await _context.Routines
                .Where(r => r.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineGoals)
                .Where(r => r.InstructorId == instructorId && r.RoutineGoals.Any(g => g.GoalId == goalId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId)
        {
            return await _context.Routines
                .Where(r => r.InstructorId == instructorId && r.LevelId == levelId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineTypes)
                .Where(r => r.InstructorId == instructorId && r.RoutineTypes.Any(t => t.TypeId == typeId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByModalityAsync(int modalityId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineModalities)
                .Where(r => r.InstructorId == instructorId && r.RoutineModalities.Any(m => m.ModalityId == modalityId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByHashtagAsync(int hashtagId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineHashtags)
                .Where(r => r.InstructorId == instructorId && r.RoutineHashtags.Any(h => h.HashtagId == hashtagId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByWorkoutAsync(int workoutId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.WorkoutRoutines)
                .Where(r => r.InstructorId == instructorId && r.WorkoutRoutines.Any(w => w.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByExerciseAsync(int exerciseId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineExercises)
                .Where(r => r.InstructorId == instructorId && r.RoutineExercises.Any(e => e.ExerciseId == exerciseId))
                .ToListAsync();
        }
    }
}
