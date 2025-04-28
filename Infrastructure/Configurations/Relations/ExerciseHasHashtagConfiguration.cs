using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class ExerciseHasHashtagConfiguration : IEntityTypeConfiguration<ExerciseHasHashtag>
    {
        public void Configure(EntityTypeBuilder<ExerciseHasHashtag> builder)
        {
            builder.ToTable("exercise_has_hashtag");

            builder.HasKey(x => new { x.ExerciseId, x.HashtagId });

            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
            builder.Property(x => x.HashtagId).HasColumnName("hashtag_id");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseHashtags)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehashashtag_exercise");

            builder.HasOne(x => x.Hashtag)
                   .WithMany(h => h.ExerciseHashtags)
                   .HasForeignKey(x => x.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehashashtag_hashtag");
        }
    }
}
