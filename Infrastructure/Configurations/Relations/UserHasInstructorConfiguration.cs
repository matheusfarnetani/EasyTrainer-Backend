using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class UserHasInstructorConfiguration : IEntityTypeConfiguration<UserHasInstructor>
    {
        public void Configure(EntityTypeBuilder<UserHasInstructor> builder)
        {
            builder.ToTable("user_has_instructor");

            builder.HasKey(x => new { x.UserId, x.InstructorId });

            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.InstructorId).HasColumnName("instructor_id");

            builder.HasOne(x => x.User)
                   .WithMany(u => u.UserInstructors)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_userhasinstructor_user");

            builder.HasOne(x => x.Instructor)
                   .WithMany(i => i.UserInstructors)
                   .HasForeignKey(x => x.InstructorId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_userhasinstructor_instructor");
        }
    }
}
