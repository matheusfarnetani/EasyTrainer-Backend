using Domain.Entities.Main;
using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Infrastructure
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Instructor> Instructors { get; }
        DbSet<Goal> Goals { get; }
        DbSet<Level> Levels { get; }
        DbSet<TrainingType> Types { get; }
        DbSet<Modality> Modalities { get; }
        DbSet<Hashtag> Hashtags { get; }
        DbSet<Workout> Workouts { get; }
        DbSet<Routine> Routines { get; }
        DbSet<Exercise> Exercises { get; }
        DbSet<RoutineHasExercise> RoutineHasExercises { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}