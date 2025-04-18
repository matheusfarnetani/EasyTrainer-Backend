using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class ExerciseHasVariationConfiguration : IEntityTypeConfiguration<ExerciseHasVariation>
    {
        public void Configure(EntityTypeBuilder<ExerciseHasVariation> builder)
        {
            builder.ToTable("exercise_has_variation");

            builder.HasKey(x => new { x.ExerciseId, x.VariationId });

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseVariations)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Variation)
                   .WithMany(e => e.IsVariationOf)
                   .HasForeignKey(x => x.VariationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
