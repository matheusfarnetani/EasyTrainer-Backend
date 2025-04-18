using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasModalityConfiguration : IEntityTypeConfiguration<WorkoutHasModality>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasModality> builder)
        {
            builder.ToTable("workout_has_modality");

            builder.HasKey(x => new { x.WorkoutId, x.ModalityId });

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutModalities)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Modality)
                   .WithMany(m => m.WorkoutModalities)
                   .HasForeignKey(x => x.ModalityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
