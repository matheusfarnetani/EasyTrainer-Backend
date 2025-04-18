using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class RoutineHasTypeConfiguration : IEntityTypeConfiguration<RoutineHasType>
    {
        public void Configure(EntityTypeBuilder<RoutineHasType> builder)
        {
            builder.ToTable("routine_has_type");

            builder.HasKey(x => new { x.RoutineId, x.TypeId });

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineTypes)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Type)
                   .WithMany(t => t.RoutineTypes)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
