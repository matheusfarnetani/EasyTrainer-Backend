using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class RoutineHasModalityConfiguration : IEntityTypeConfiguration<RoutineHasModality>
    {
        public void Configure(EntityTypeBuilder<RoutineHasModality> builder)
        {
            builder.ToTable("routine_has_modality");

            builder.HasKey(x => new { x.RoutineId, x.ModalityId });

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineModalities)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Modality)
                   .WithMany(m => m.RoutineModalities)
                   .HasForeignKey(x => x.ModalityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
