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

            builder.Property(x => x.RoutineId).HasColumnName("routine_id");
            builder.Property(x => x.TypeId).HasColumnName("type_id");

            builder.HasOne(x => x.Routine)
                   .WithMany(r => r.RoutineTypes)
                   .HasForeignKey(x => x.RoutineId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehastype_routine");

            builder.HasOne(x => x.Type)
                   .WithMany(t => t.RoutineTypes)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_routinehastype_type");
        }
    }
}
