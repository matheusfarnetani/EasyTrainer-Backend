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

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutUsers)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                   .WithMany(u => u.WorkoutUsers)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
