using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class RoutineHasHashtagConfiguration : IEntityTypeConfiguration<RoutineHasHashtag>
    {
        public void Configure(EntityTypeBuilder<RoutineHasHashtag> builder)
        {
            builder.ToTable("routine_has_hashtag");

            builder.HasKey(x => new { x.RoutineId, x.HashtagId });

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineHashtags)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Hashtag)
                   .WithMany(h => h.RoutineHashtags)
                   .HasForeignKey(x => x.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
