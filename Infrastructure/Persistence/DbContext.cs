using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<TrainingType> Types { get; set; }
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<WorkoutHasUser> WorkoutHasUsers { get; set; }
        public DbSet<WorkoutHasRoutine> WorkoutHasRoutines { get; set; }
        public DbSet<WorkoutHasExercise> WorkoutHasExercises { get; set; }
        public DbSet<WorkoutHasGoal> WorkoutHasGoals { get; set; }
        public DbSet<WorkoutHasType> WorkoutHasTypes { get; set; }
        public DbSet<WorkoutHasModality> WorkoutHasModalities { get; set; }
        public DbSet<WorkoutHasHashtag> WorkoutHasHashtags { get; set; }

        public DbSet<RoutineHasGoal> RoutineHasGoals { get; set; }
        public DbSet<RoutineHasType> RoutineHasTypes { get; set; }
        public DbSet<RoutineHasModality> RoutineHasModalities { get; set; }
        public DbSet<RoutineHasHashtag> RoutineHasHashtags { get; set; }
        public DbSet<RoutineHasExercise> RoutineHasExercises { get; set; }

        public DbSet<ExerciseHasGoal> ExerciseHasGoals { get; set; }
        public DbSet<ExerciseHasType> ExerciseHasTypes { get; set; }
        public DbSet<ExerciseHasModality> ExerciseHasModalities { get; set; }
        public DbSet<ExerciseHasHashtag> ExerciseHasHashtags { get; set; }
        public DbSet<ExerciseHasVariation> ExerciseHasVariations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
