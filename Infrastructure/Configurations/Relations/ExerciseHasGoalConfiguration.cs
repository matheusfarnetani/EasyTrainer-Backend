using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class ExerciseHasGoalConfiguration : IEntityTypeConfiguration<ExerciseHasGoal>
    {
        public void Configure(EntityTypeBuilder<ExerciseHasGoal> builder)
        {
            builder.ToTable("exercise_has_goal");

            builder.HasKey(x => new { x.ExerciseId, x.GoalId });

            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
            builder.Property(x => x.GoalId).HasColumnName("goal_id");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseGoals)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasgoal_exercise");

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.ExerciseGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasgoal_goal");
        }
    }
}
