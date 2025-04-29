using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Entities
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("exercise");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Description)
                   .HasMaxLength(300);

            builder.Property(e => e.Equipment)
                   .HasMaxLength(100)
                   .HasColumnName("equipment");

            builder.Property(e => e.Duration)
                   .IsRequired()
                   .HasColumnType("Time")
                   .HasColumnName("duration");

            builder.Property(e => e.Repetition)
                   .IsRequired()
                   .HasColumnName("repetition");

            builder.Property(e => e.Sets)
                   .IsRequired()
                   .HasColumnName("sets");

            builder.Property(e => e.RestTime)
                   .IsRequired()
                   .HasColumnType("Time")
                   .HasColumnName("rest_time");

            builder.Property(e => e.BodyPart)
                   .HasMaxLength(100)
                   .HasColumnName("body_part");

            builder.Property(e => e.VideoUrl)
                   .HasMaxLength(300)
                   .HasColumnName("video_url");

            builder.Property(e => e.ImageUrl)
                   .HasMaxLength(300)
                   .HasColumnName("image_url");

            builder.Property(e => e.Steps)
                   .HasMaxLength(1000)
                   .HasColumnName("steps");

            builder.Property(e => e.Contraindications)
                   .HasMaxLength(500)
                   .HasColumnName("contraindications");

            builder.Property(e => e.SafetyTips)
                   .HasMaxLength(500)
                   .HasColumnName("safety_tips");

            builder.Property(e => e.CommonMistakes)
                   .HasMaxLength(500)
                   .HasColumnName("common_mistakes");

            builder.Property(e => e.IndicatedFor)
                   .HasMaxLength(500)
                   .HasColumnName("indicated_for");

            builder.Property(e => e.CaloriesBurnedEstimate)
                   .HasColumnType("float")
                   .HasColumnName("calories_burned_estimate");

            builder.Property(e => e.InstructorId)
                   .HasColumnName("instructor_id");

            builder.Property(e => e.LevelId)
                   .HasColumnName("level_id");

            builder.HasOne(e => e.Instructor)
                   .WithMany(i => i.Exercises)
                   .HasForeignKey(e => e.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_exercise_instructor");

            builder.HasOne(e => e.Level)
                   .WithMany(l => l.Exercises)
                   .HasForeignKey(e => e.LevelId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_exercise_level");
        }
    }
}