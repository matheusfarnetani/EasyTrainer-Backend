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

            builder.HasOne(x => x.User)
                   .WithMany(u => u.UserInstructors)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Instructor)
                   .WithMany(i => i.UserInstructors)
                   .HasForeignKey(x => x.InstructorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
