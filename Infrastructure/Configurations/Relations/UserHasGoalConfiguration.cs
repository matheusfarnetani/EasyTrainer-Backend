using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Relations
{
    public class UserHasGoalConfiguration : IEntityTypeConfiguration<UserHasGoal>
    {
        public void Configure(EntityTypeBuilder<UserHasGoal> builder)
        {
            builder.ToTable("user_has_goal");

            builder.HasKey(x => new { x.UserId, x.GoalId });

            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.GoalId).HasColumnName("goal_id");

            builder.HasOne(x => x.User)
                   .WithMany(u => u.UserGoals)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_userhasgoal_user");

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.UserGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_userhasgoal_goal");
        }
    }
}
