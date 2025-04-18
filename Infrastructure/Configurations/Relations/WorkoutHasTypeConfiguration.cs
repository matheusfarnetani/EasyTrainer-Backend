using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Relations
{
    public class WorkoutHasTypeConfiguration : IEntityTypeConfiguration<WorkoutHasType>
    {
        public void Configure(EntityTypeBuilder<WorkoutHasType> builder)
        {
            builder.ToTable("workout_has_type");

            builder.HasKey(x => new { x.WorkoutId, x.TypeId });

            builder.HasOne(x => x.Workout)
                   .WithMany(w => w.WorkoutTypes)
                   .HasForeignKey(x => x.WorkoutId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Type)
                   .WithMany(t => t.WorkoutTypes)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
