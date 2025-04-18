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

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutRoutines)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.WorkoutRoutines)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
