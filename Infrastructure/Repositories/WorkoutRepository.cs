using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
    {
        private readonly AppDbContext _context;

        public WorkoutRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<Workout?> GetByIdAsync(int id)
        {
            return await _context.Workouts
                .Include(w => w.Instructor)
                .Include(w => w.Level)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public new async Task DeleteByIdAsync(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout is not null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Workouts.AnyAsync(w => w.Id == id);
        }

        public async Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId)
        {
            return await _context.Workouts
                .Where(w => w.Id == workoutId)
                .Select(w => w.Instructor)
                .FirstOrDefaultAsync();
        }

        public async Task AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutGoals)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutGoals.Any(g => g.GoalId == goalId))
                return;

            workout.WorkoutGoals.Add(new WorkoutHasGoal { WorkoutId = workoutId, GoalId = goalId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            var relation = await _context.WorkoutHasGoals
                .Include(wg => wg.Workout)
                .FirstOrDefaultAsync(wg => wg.WorkoutId == workoutId && wg.GoalId == goalId && wg.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasGoals.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutTypes)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutTypes.Any(t => t.TypeId == typeId))
                return;

            workout.WorkoutTypes.Add(new WorkoutHasType { WorkoutId = workoutId, TypeId = typeId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            var relation = await _context.WorkoutHasTypes
                .Include(wt => wt.Workout)
                .FirstOrDefaultAsync(wt =>
                    wt.WorkoutId == workoutId &&
                    wt.TypeId == typeId &&
                    wt.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasTypes.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutModalities)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutModalities.Any(m => m.ModalityId == modalityId))
                return;

            workout.WorkoutModalities.Add(new WorkoutHasModality { WorkoutId = workoutId, ModalityId = modalityId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            var relation = await _context.WorkoutHasModalities
                .Include(wm => wm.Workout)
                .FirstOrDefaultAsync(wm =>
                    wm.WorkoutId == workoutId &&
                    wm.ModalityId == modalityId &&
                    wm.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasModalities.Remove(relation);
            await _context.SaveChangesAsync();
        }
        public async Task AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutHashtags)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutHashtags.Any(h => h.HashtagId == hashtagId))
                return;

            workout.WorkoutHashtags.Add(new WorkoutHasHashtag { WorkoutId = workoutId, HashtagId = hashtagId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            var relation = await _context.WorkoutHasHashtags
                .Include(wh => wh.Workout)
                .FirstOrDefaultAsync(wh =>
                    wh.WorkoutId == workoutId &&
                    wh.HashtagId == hashtagId &&
                    wh.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasHashtags.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutRoutines)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutRoutines.Any(r => r.RoutineId == routineId))
                return;

            workout.WorkoutRoutines.Add(new WorkoutHasRoutine { WorkoutId = workoutId, RoutineId = routineId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            var relation = await _context.WorkoutHasRoutines
                .Include(wr => wr.Workout)
                .FirstOrDefaultAsync(wr =>
                    wr.WorkoutId == workoutId &&
                    wr.RoutineId == routineId &&
                    wr.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasRoutines.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(w => w.Id == workoutId && w.InstructorId == instructorId);

            if (workout is null || workout.WorkoutExercises.Any(e => e.ExerciseId == exerciseId))
                return;

            workout.WorkoutExercises.Add(new WorkoutHasExercise { WorkoutId = workoutId, ExerciseId = exerciseId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            var relation = await _context.WorkoutHasExercises
                .Include(we => we.Workout)
                .FirstOrDefaultAsync(we =>
                    we.WorkoutId == workoutId &&
                    we.ExerciseId == exerciseId &&
                    we.Workout.InstructorId == instructorId);

            if (relation is null)
                return;

            _context.WorkoutHasExercises.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId)
        {
            return await _context.Workouts
                .Include(w => w.WorkoutUsers)
                .Where(w => w.WorkoutUsers.Any(u => u.UserId == userId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByInstructorIdAsync(int instructorId)
        {
            return await _context.Workouts
                .Where(w => w.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByGoalIdAsync(int goalId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutGoals)
                .AsQueryable();

            query = query.Where(w => w.WorkoutGoals.Any(g => g.GoalId == goalId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByLevelIdAsync(int levelId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts.AsQueryable();

            query = query.Where(w => w.LevelId == levelId);

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByTypeIdAsync(int typeId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutTypes)
                .AsQueryable();

            query = query.Where(w => w.WorkoutTypes.Any(t => t.TypeId == typeId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByModalityIdAsync(int modalityId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutModalities)
                .AsQueryable();

            query = query.Where(w => w.WorkoutModalities.Any(m => m.ModalityId == modalityId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByHashtagIdAsync(int hashtagId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutHashtags)
                .AsQueryable();

            query = query.Where(w => w.WorkoutHashtags.Any(h => h.HashtagId == hashtagId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByRoutineIdAsync(int routineId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutRoutines)
                .AsQueryable();

            query = query.Where(w => w.WorkoutRoutines.Any(r => r.RoutineId == routineId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workout>> GetWorkoutsByExerciseIdAsync(int exerciseId, int? instructorId = null, int? userId = null)
        {
            var query = _context.Workouts
                .Include(w => w.WorkoutExercises)
                .AsQueryable();

            query = query.Where(w => w.WorkoutExercises.Any(e => e.ExerciseId == exerciseId));

            if (instructorId.HasValue)
                query = query.Where(w => w.InstructorId == instructorId.Value);

            if (userId.HasValue)
                query = query.Where(w => w.WorkoutUsers.Any(u => u.UserId == userId.Value));

            return await query.ToListAsync();
        }

    }
}
