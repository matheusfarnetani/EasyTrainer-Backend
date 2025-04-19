using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises
                .Include(e => e.Instructor)
                .Include(e => e.Level)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public new async Task DeleteByIdAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise is not null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Exercises.AnyAsync(e => e.Id == id);
        }

        public async Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId)
        {
            return await _context.Exercises
                .Where(e => e.Id == exerciseId)
                .Select(e => e.Instructor)
                .FirstOrDefaultAsync();
        }

        public async Task AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseGoals)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.InstructorId == instructorId);

            if (exercise is null || exercise.ExerciseGoals.Any(g => g.GoalId == goalId))
                return;

            exercise.ExerciseGoals.Add(new ExerciseHasGoal { ExerciseId = exerciseId, GoalId = goalId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            var relation = await _context.ExerciseHasGoals
                .Include(eg => eg.Exercise)
                .FirstOrDefaultAsync(eg =>
                    eg.ExerciseId == exerciseId &&
                    eg.GoalId == goalId &&
                    eg.Exercise.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.ExerciseHasGoals.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseTypes)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.InstructorId == instructorId);

            if (exercise is null || exercise.ExerciseTypes.Any(t => t.TypeId == typeId))
                return;

            exercise.ExerciseTypes.Add(new ExerciseHasType { ExerciseId = exerciseId, TypeId = typeId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            var relation = await _context.ExerciseHasTypes
                .Include(et => et.Exercise)
                .FirstOrDefaultAsync(et =>
                    et.ExerciseId == exerciseId &&
                    et.TypeId == typeId &&
                    et.Exercise.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.ExerciseHasTypes.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseModalities)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.InstructorId == instructorId);

            if (exercise is null || exercise.ExerciseModalities.Any(m => m.ModalityId == modalityId))
                return;

            exercise.ExerciseModalities.Add(new ExerciseHasModality { ExerciseId = exerciseId, ModalityId = modalityId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            var relation = await _context.ExerciseHasModalities
                .Include(em => em.Exercise)
                .FirstOrDefaultAsync(em =>
                    em.ExerciseId == exerciseId &&
                    em.ModalityId == modalityId &&
                    em.Exercise.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.ExerciseHasModalities.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseHashtags)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.InstructorId == instructorId);

            if (exercise is null || exercise.ExerciseHashtags.Any(h => h.HashtagId == hashtagId))
                return;

            exercise.ExerciseHashtags.Add(new ExerciseHasHashtag { ExerciseId = exerciseId, HashtagId = hashtagId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            var relation = await _context.ExerciseHasHashtags
                .Include(eh => eh.Exercise)
                .FirstOrDefaultAsync(eh =>
                    eh.ExerciseId == exerciseId &&
                    eh.HashtagId == hashtagId &&
                    eh.Exercise.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.ExerciseHasHashtags.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.ExerciseVariations)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && e.InstructorId == instructorId);

            if (exercise is null || exercise.ExerciseVariations.Any(v => v.VariationId == variationId))
                return;

            exercise.ExerciseVariations.Add(new ExerciseHasVariation { ExerciseId = exerciseId, VariationId = variationId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            var relation = await _context.ExerciseHasVariations
                .Include(ev => ev.Exercise)
                .FirstOrDefaultAsync(ev =>
                    ev.ExerciseId == exerciseId &&
                    ev.VariationId == variationId &&
                    ev.Exercise.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.ExerciseHasVariations.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId)
        {
            return await _context.Exercises
                .Where(e => e.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.ExerciseGoals)
                .Where(e => e.InstructorId == instructorId && e.ExerciseGoals.Any(g => g.GoalId == goalId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId)
        {
            return await _context.Exercises
                .Where(e => e.LevelId == levelId && e.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int typeId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.ExerciseTypes)
                .Where(e => e.InstructorId == instructorId && e.ExerciseTypes.Any(t => t.TypeId == typeId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByModalityIdAsync(int modalityId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.ExerciseModalities)
                .Where(e => e.InstructorId == instructorId && e.ExerciseModalities.Any(m => m.ModalityId == modalityId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.ExerciseHashtags)
                .Where(e => e.InstructorId == instructorId && e.ExerciseHashtags.Any(h => h.HashtagId == hashtagId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByRoutineIdAsync(int routineId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.RoutineExercises)
                .Where(e => e.InstructorId == instructorId && e.RoutineExercises.Any(r => r.RoutineId == routineId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.WorkoutExercises)
                .Where(e => e.InstructorId == instructorId && e.WorkoutExercises.Any(w => w.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int exerciseId, int instructorId)
        {
            return await _context.Exercises
                .Include(e => e.IsVariationOf)
                .Where(e => e.InstructorId == instructorId && e.IsVariationOf.Any(v => v.ExerciseId == exerciseId))
                .ToListAsync();
        }
    }
}
