using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Entities
{
    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.ToTable("workouts");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
            builder.Property(w => w.Description).HasMaxLength(300);
            builder.Property(w => w.ImageUrl).HasMaxLength(300);
            builder.Property(w => w.Duration).IsRequired();
            builder.Property(w => w.NumberOfDays).IsRequired();
            builder.Property(w => w.Indoor).IsRequired();

            builder.HasOne(w => w.Instructor)
                   .WithMany()
                   .HasForeignKey(w => w.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Level)
                   .WithMany()
                   .HasForeignKey(w => w.LevelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
