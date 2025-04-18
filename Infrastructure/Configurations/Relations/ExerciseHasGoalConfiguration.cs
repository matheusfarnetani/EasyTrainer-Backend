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

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseGoals)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.ExerciseGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
