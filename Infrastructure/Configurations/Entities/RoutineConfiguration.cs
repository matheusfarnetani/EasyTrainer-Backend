using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities
{
    public class RoutineConfiguration : IEntityTypeConfiguration<Routine>
    {
        public void Configure(EntityTypeBuilder<Routine> builder)
        {
            builder.ToTable("routine");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.Description)
                   .HasMaxLength(300);

            builder.Property(r => r.ImageUrl)
                   .HasMaxLength(300);

            builder.Property(r => r.Duration)
                   .HasColumnName("duration");

            builder.HasOne(r => r.Instructor)
                   .WithMany()
                   .HasForeignKey(r => r.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_routine_instructor");

            builder.HasOne(r => r.Level)
                   .WithMany()
                   .HasForeignKey(r => r.LevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_routine_level");
        }
    }
}
