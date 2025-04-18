using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Entities
{
    public class RoutineConfiguration : IEntityTypeConfiguration<Routine>
    {
        public void Configure(EntityTypeBuilder<Routine> builder)
        {
            builder.ToTable("routines");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Description).HasMaxLength(300);
            builder.Property(r => r.ImageUrl).HasMaxLength(300);
            builder.Property(r => r.Duration).IsRequired();

            builder.HasOne(r => r.Instructor)
                   .WithMany()
                   .HasForeignKey(r => r.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Level)
                   .WithMany()
                   .HasForeignKey(r => r.LevelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
