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

            builder.Property(x => x.RoutineId).HasColumnName("routine_id");
            builder.Property(x => x.HashtagId).HasColumnName("hashtag_id");

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineHashtags)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehashashtag_routine");

            builder.HasOne(x => x.Hashtag)
                   .WithMany(h => h.RoutineHashtags)
                   .HasForeignKey(x => x.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehashashtag_hashtag");
        }
    }
}
