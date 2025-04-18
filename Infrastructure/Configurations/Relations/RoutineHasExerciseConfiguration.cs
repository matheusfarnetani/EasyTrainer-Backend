using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class RoutineHasExerciseConfiguration : IEntityTypeConfiguration<RoutineHasExercise>
    {
        public void Configure(EntityTypeBuilder<RoutineHasExercise> builder)
        {
            builder.ToTable("routine_has_exercise");

            builder.HasKey(x => new { x.RoutineId, x.ExerciseId });

            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.Sets).IsRequired();
            builder.Property(x => x.Reps).IsRequired();
            builder.Property(x => x.RestTime).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(300);
            builder.Property(x => x.Day).IsRequired();
            builder.Property(x => x.Week).IsRequired();
            builder.Property(x => x.IsOptional).IsRequired();

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineExercises)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.RoutineExercises)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
