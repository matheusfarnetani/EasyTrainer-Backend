using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class UserHasGoalConfiguration : IEntityTypeConfiguration<UserHasGoal>
    {
        public void Configure(EntityTypeBuilder<UserHasGoal> builder)
        {
            builder.ToTable("user_has_goal");

            builder.HasKey(x => new { x.UserId, x.GoalId });

            builder.HasOne(x => x.User)
                   .WithMany(u => u.UserGoals)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.UserGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
