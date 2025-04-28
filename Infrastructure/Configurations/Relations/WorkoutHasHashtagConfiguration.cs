using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasHashtagConfiguration : IEntityTypeConfiguration<WorkoutHasHashtag>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasHashtag> builder)
        {
            builder.ToTable("workout_has_hashtag");

            builder.HasKey(x => new { x.WorkoutId, x.HashtagId });

            builder.Property(x => x.WorkoutId).HasColumnName("workout_id");
            builder.Property(x => x.HashtagId).HasColumnName("hashtag_id");

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutHashtags)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthashashtag_workout");

            builder.HasOne(x => x.Hashtag)
                   .WithMany(h => h.WorkoutHashtags)
                   .HasForeignKey(x => x.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_workouthashashtag_hashtag");
        }
    }
}
