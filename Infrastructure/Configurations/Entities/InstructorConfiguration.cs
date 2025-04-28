using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("instructor");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(i => i.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(i => i.MobileNumber)
                   .HasMaxLength(20)
                   .HasColumnName("mobile_number");

            builder.Property(i => i.Birthday)
                   .HasColumnName("birthday");

            builder.Property(i => i.Gender)
                   .HasColumnName("gender");

            builder.Property(i => i.Password)
                   .IsRequired()
                   .HasColumnName("password");

            builder.HasIndex(i => i.Email).IsUnique();

            builder.HasMany(i => i.Workouts)
                   .WithOne(w => w.Instructor)
                   .HasForeignKey(w => w.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_workout_instructor");

        }
    }
}
