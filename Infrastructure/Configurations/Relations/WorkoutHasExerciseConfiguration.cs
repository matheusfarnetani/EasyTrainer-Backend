using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasExerciseConfiguration : IEntityTypeConfiguration<WorkoutHasExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasExercise> builder)
        {
            builder.ToTable("workout_has_exercise");

            builder.HasKey(x => new { x.WorkoutId, x.ExerciseId });

            builder.Property(x => x.WorkoutId).HasColumnName("workout_id");
            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutExercises)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasexercise_workout");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.WorkoutExercises)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasexercise_exercise");
        }
    }
}
