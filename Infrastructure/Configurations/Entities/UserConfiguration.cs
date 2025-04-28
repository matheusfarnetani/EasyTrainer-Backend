using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.MobileNumber)
                   .HasMaxLength(20)
                   .HasColumnName("mobile_number");

            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasColumnName("password");

            builder.Property(u => u.Gender)
                   .HasColumnName("gender");

            builder.Property(u => u.Birthday)
                   .HasColumnName("birthday");

            builder.Property(u => u.Weight)
                   .HasColumnName("weight")
                   .HasColumnType("decimal(5,2)");

            builder.Property(u => u.Height)
                   .HasColumnName("height")
                   .HasColumnType("decimal(4,2)");

            builder.Property(u => u.LevelId)
                   .HasColumnName("level_id");

            builder.Property(u => u.InstructorId)
                   .HasColumnName("instructor_id");

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasOne(u => u.Level)
                   .WithMany(l => l.Users)
                   .HasForeignKey(u => u.LevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_user_level1");

            builder.HasOne(u => u.Instructor)
                   .WithMany()
                   .HasForeignKey(u => u.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_user_instructor1");
        }
    }
}
