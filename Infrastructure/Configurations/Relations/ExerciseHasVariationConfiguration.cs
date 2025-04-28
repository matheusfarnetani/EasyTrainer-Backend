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

            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
            builder.Property(x => x.VariationId).HasColumnName("variation_id");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseVariations)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasvariation_exercise");

            builder.HasOne(x => x.Variation)
                   .WithMany(e => e.IsVariationOf)
                   .HasForeignKey(x => x.VariationId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasvariation_variation");
        }
    }
}
