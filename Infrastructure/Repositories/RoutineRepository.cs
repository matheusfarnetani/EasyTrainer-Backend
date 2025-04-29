using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoutineRepository(AppDbContext context) : GenericRepository<Routine>(context), IRoutineRepository
    {
        private readonly AppDbContext _context = context;

        // Basic Queries
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Routines.AnyAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Routine>> GetAllWithIncludesByInstructorIdAsync(int instructorId)
        {
            return await _context.Routines
                .Where(r => r.InstructorId == instructorId)
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineExercises)
                    .ThenInclude(re => re.Exercise)
                .ToListAsync();
        }


        public async Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId)
        {
            return await _context.Routines
                .Where(r => r.InstructorId == instructorId)
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineExercises)
                    .ThenInclude(re => re.Exercise)
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId)
        {
            return await _context.Routines
                .Where(r => r.InstructorId == instructorId && r.RoutineGoals.Any(g => g.GoalId == goalId))
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineExercises)
                    .ThenInclude(re => re.Exercise)
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Where(r => r.InstructorId == instructorId && r.LevelId == levelId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Where(r => r.InstructorId == instructorId && r.RoutineTypes.Any(t => t.TypeId == typeId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineHashtags)
                .Where(r => r.InstructorId == instructorId && r.RoutineModalities.Any(m => m.ModalityId == modalityId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Where(r => r.InstructorId == instructorId && r.RoutineHashtags.Any(h => h.HashtagId == hashtagId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineExercises)
                .Where(r => r.InstructorId == instructorId && r.WorkoutRoutines.Any(w => w.WorkoutId == workoutId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetRoutinesByExerciseIdAsync(int exerciseId, int instructorId)
        {
            return await _context.Routines
                .Include(r => r.RoutineGoals)
                .Include(r => r.RoutineTypes)
                .Include(r => r.RoutineModalities)
                .Include(r => r.RoutineHashtags)
                .Include(r => r.RoutineExercises)
                .Where(r => r.InstructorId == instructorId && r.RoutineExercises.Any(e => e.ExerciseId == exerciseId))
                .ToListAsync();
        }

        // Relationship: GOAL
        public async Task AddGoalToRoutineAsync(int routineId, int goalId, int instructorId)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineGoals)
                .FirstOrDefaultAsync(r => r.Id == routineId && r.InstructorId == instructorId);

            if (routine is null || routine.RoutineGoals.Any(g => g.GoalId == goalId))
                return;

            routine.RoutineGoals.Add(new RoutineHasGoal { RoutineId = routineId, GoalId = goalId });
        }

        public async Task RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId)
        {
            var relation = await _context.RoutineHasGoals
                .Include(rg => rg.Routine)
                .FirstOrDefaultAsync(rg =>
                    rg.RoutineId == routineId &&
                    rg.GoalId == goalId &&
                    rg.Routine.InstructorId == instructorId);

            if (relation is null) return;

            _context.RoutineHasGoals.Remove(relation);
        }

        // Relationship: TYPE
        public async Task AddTypeToRoutineAsync(int routineId, int typeId, int instructorId)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineTypes)
                .FirstOrDefaultAsync(r => r.Id == routineId && r.InstructorId == instructorId);

            if (routine is null || routine.RoutineTypes.Any(t => t.TypeId == typeId))
                return;

            routine.RoutineTypes.Add(new RoutineHasType { RoutineId = routineId, TypeId = typeId });
        }

        public async Task RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId)
        {
            var relation = await _context.RoutineHasTypes
                .Include(rt => rt.Routine)
                .FirstOrDefaultAsync(rt =>
                    rt.RoutineId == routineId &&
                    rt.TypeId == typeId &&
                    rt.Routine.InstructorId == instructorId);

            if (relation is null) return;

            _context.RoutineHasTypes.Remove(relation);
        }

        // Relationship: MODALITY
        public async Task AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineModalities)
                .FirstOrDefaultAsync(r => r.Id == routineId && r.InstructorId == instructorId);

            if (routine is null || routine.RoutineModalities.Any(m => m.ModalityId == modalityId))
                return;

            routine.RoutineModalities.Add(new RoutineHasModality { RoutineId = routineId, ModalityId = modalityId });
        }

        public async Task RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            var relation = await _context.RoutineHasModalities
                .Include(rm => rm.Routine)
                .FirstOrDefaultAsync(rm =>
                    rm.RoutineId == routineId &&
                    rm.ModalityId == modalityId &&
                    rm.Routine.InstructorId == instructorId);

            if (relation is null) return;

            _context.RoutineHasModalities.Remove(relation);
        }

        // Relationship: HASHTAG
        public async Task AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineHashtags)
                .FirstOrDefaultAsync(r => r.Id == routineId && r.InstructorId == instructorId);

            if (routine is null || routine.RoutineHashtags.Any(h => h.HashtagId == hashtagId))
                return;

            routine.RoutineHashtags.Add(new RoutineHasHashtag { RoutineId = routineId, HashtagId = hashtagId });
        }

        public async Task RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            var relation = await _context.RoutineHasHashtags
                .Include(rh => rh.Routine)
                .FirstOrDefaultAsync(rh =>
                    rh.RoutineId == routineId &&
                    rh.HashtagId == hashtagId &&
                    rh.Routine.InstructorId == instructorId);

            if (relation is null) return;

            _context.RoutineHasHashtags.Remove(relation);
        }

        // Relationship: EXERCISE
        public async Task AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            var routine = await _context.Routines
                .Include(r => r.RoutineExercises)
                .FirstOrDefaultAsync(r => r.Id == routineId && r.InstructorId == instructorId);

            if (routine is null || routine.RoutineExercises.Any(e => e.ExerciseId == exerciseId))
                return;

            routine.RoutineExercises.Add(new RoutineHasExercise { RoutineId = routineId, ExerciseId = exerciseId });
        }

        public async Task RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            var relation = await _context.RoutineHasExercises
                .Include(re => re.Routine)
                .FirstOrDefaultAsync(re =>
                    re.RoutineId == routineId &&
                    re.ExerciseId == exerciseId &&
                    re.Routine.InstructorId == instructorId);

            if (relation is null) return;

            _context.RoutineHasExercises.Remove(relation);
        }

        // Instructor Getter
        public async Task<Instructor?> GetInstructorByRoutineIdAsync(int routineId)
        {
            return await _context.Routines
                .Where(r => r.Id == routineId)
                .Select(r => r.Instructor)
                .FirstOrDefaultAsync();
        }
    }
}
