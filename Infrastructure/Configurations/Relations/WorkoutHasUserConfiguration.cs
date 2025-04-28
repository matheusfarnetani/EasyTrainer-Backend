using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasUserConfiguration : IEntityTypeConfiguration<WorkoutHasUser>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasUser> builder)
        {
            builder.ToTable("workout_has_user");

            builder.HasKey(x => new { x.WorkoutId, x.UserId });

            builder.Property(x => x.WorkoutId)
                   .HasColumnName("workout_id");

            builder.Property(x => x.UserId)
                   .HasColumnName("user_id");

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutUsers)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasuser_workout");

            builder.HasOne(x => x.User)
                   .WithMany(u => u.WorkoutUsers)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthasuser_user");
        }
    }
}
