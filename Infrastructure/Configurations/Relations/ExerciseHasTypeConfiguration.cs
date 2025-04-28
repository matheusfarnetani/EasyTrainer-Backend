using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class ExerciseHasTypeConfiguration : IEntityTypeConfiguration<ExerciseHasType>
    {
        public void Configure(EntityTypeBuilder<ExerciseHasType> builder)
        {
            builder.ToTable("exercise_has_type");

            builder.HasKey(x => new { x.ExerciseId, x.TypeId });

            builder.Property(x => x.ExerciseId).HasColumnName("exercise_id");
            builder.Property(x => x.TypeId).HasColumnName("type_id");

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseTypes)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehastype_exercise");

            builder.HasOne(x => x.Type)
                   .WithMany(t => t.ExerciseTypes)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_exercisehastype_type");
        }
    }
}
