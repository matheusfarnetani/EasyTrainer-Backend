using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities
{
    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.ToTable("workout");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(w => w.Description)
                   .HasMaxLength(300);

            builder.Property(w => w.ImageUrl)
                   .HasMaxLength(300)
                   .HasColumnName("image_url");

            builder.Property(w => w.Duration)
                   .HasColumnName("duration");

            builder.Property(w => w.NumberOfDays)
                   .IsRequired()
                   .HasColumnName("number_of_days");

            builder.Property(w => w.Indoor)
                   .IsRequired()
                   .HasColumnName("indoor");

            builder.Property(w => w.InstructorId)
                .IsRequired()
                .HasColumnName("instructor_id");

            builder.Property(w => w.LevelId)
                .IsRequired()
                .HasColumnName("level_id");

            builder.HasOne(w => w.Instructor)
                   .WithMany(i => i.Workouts)
                   .HasForeignKey(w => w.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_workout_instructor");

            builder.HasOne(w => w.Level)
                   .WithMany(l => l.Workouts)
                   .HasForeignKey(w => w.LevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_workout_level");
        }
    }
}
