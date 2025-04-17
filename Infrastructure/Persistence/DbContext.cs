using Domain.Entities.Main;
using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets: entidades principais
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

        // DbSets: relações
        public DbSet<RoutineHasExercise> RoutineHasExercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chave composta para a relação many-to-many personalizada
            modelBuilder.Entity<RoutineHasExercise>()
                .HasKey(x => new { x.RoutineId, x.ExerciseId });

            modelBuilder.Entity<RoutineHasExercise>()
                .HasOne(rhe => rhe.Routine)
                .WithMany(r => r.UsersWorkout)
                .HasForeignKey(rhe => rhe.RoutineId);

            modelBuilder.Entity<RoutineHasExercise>()
                .HasOne(rhe => rhe.Exercise)
                .WithMany(e => e.Routines)
                .HasForeignKey(rhe => rhe.ExerciseId);
        }
    }
}
