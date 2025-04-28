using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasRoutineConfiguration : IEntityTypeConfiguration<WorkoutHasRoutine>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasRoutine> builder)
        {
            builder.ToTable("workout_has_routine");

            builder.HasKey(x => new { x.WorkoutId, x.RoutineId });

            builder.Property(x => x.WorkoutId).HasColumnName("workout_id");
            builder.Property(x => x.RoutineId).HasColumnName("routine_id");

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutRoutines)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasroutine_workout");

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.WorkoutRoutines)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasroutine_routine");
        }
    }
}
