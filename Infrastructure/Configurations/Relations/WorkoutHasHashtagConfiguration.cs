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

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutHashtags)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Hashtag)
                   .WithMany(h => h.WorkoutHashtags)
                   .HasForeignKey(x => x.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
