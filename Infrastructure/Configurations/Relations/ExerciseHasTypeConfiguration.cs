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

            builder.HasOne(x => x.Exercise)
                   .WithMany(e => e.ExerciseTypes)
                   .HasForeignKey(x => x.ExerciseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Type)
                   .WithMany(t => t.ExerciseTypes)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
