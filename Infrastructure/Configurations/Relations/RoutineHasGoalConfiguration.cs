﻿using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class RoutineHasGoalConfiguration : IEntityTypeConfiguration<RoutineHasGoal>
    {
        public void Configure(EntityTypeBuilder<RoutineHasGoal> builder)
        {
            builder.ToTable("routine_has_goal");

            builder.HasKey(x => new { x.RoutineId, x.GoalId });

            builder.Property(x => x.RoutineId).HasColumnName("routine_id");
            builder.Property(x => x.GoalId).HasColumnName("goal_id");

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineGoals)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehasgoal_routine");

            builder.HasOne(x => x.Goal)
                   .WithMany(g => g.RoutineGoals)
                   .HasForeignKey(x => x.GoalId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehasgoal_goal");
        }
    }
}
