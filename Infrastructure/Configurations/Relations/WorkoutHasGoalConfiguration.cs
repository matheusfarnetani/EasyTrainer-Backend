using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasGoalConfiguration : IEntityTypeConfiguration<WorkoutHasGoal>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasGoal> builder)
        {
            builder.ToTable("workout_has_goal");

            builder.HasKey(x => new { x.WorkoutId, x.GoalId });

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutGoals)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.WorkoutGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
