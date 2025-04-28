using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class ExerciseHasModalityConfiguration : IEntityTypeConfiguration<ExerciseHasModality>
    {
        public void Configure(EntityTypeBuilder<ExerciseHasModality> builder)
        {
            builder.ToTable("exercise_has_modality");

            builder.HasKey(x => new { x.ExerciseId, x.ModalityId });

            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
            builder.Property(x => x.ModalityId).HasColumnName("modality_id");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseModalities)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasmodality_exercise");

            builder.HasOne(x => x.Modality)
                   .WithMany(m => m.ExerciseModalities)
                   .HasForeignKey(x => x.ModalityId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehasmodality_modality");
        }
    }
}
