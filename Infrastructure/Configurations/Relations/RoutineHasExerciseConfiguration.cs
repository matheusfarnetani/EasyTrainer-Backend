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

            builder.Property(x => x.RoutineId).HasColumnName("routine_id");
            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");

            builder.Property(x => x.Order).IsRequired().HasColumnName("order");
            builder.Property(x => x.Sets).IsRequired().HasColumnName("sets");
            builder.Property(x => x.Reps).IsRequired().HasColumnName("reps");
            builder.Property(x => x.RestTime).IsRequired().HasColumnName("rest_time");  
            builder.Property(x => x.Note).HasMaxLength(300).HasColumnName("note");
            builder.Property(x => x.Day).IsRequired().HasColumnName("day");
            builder.Property(x => x.Week).IsRequired().HasColumnName("week");
            builder.Property(x => x.IsOptional).IsRequired().HasColumnName("is_optional");


            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineExercises)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehasexercise_routine");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.RoutineExercises)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehasexercise_exercise");
        }
    }
}
