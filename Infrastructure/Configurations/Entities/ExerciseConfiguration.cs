﻿using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Entities
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("exercises");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(300);
            builder.Property(e => e.Equipment).HasMaxLength(100);
            builder.Property(e => e.Duration).IsRequired();
            builder.Property(e => e.Repetition).IsRequired();
            builder.Property(e => e.Sets).IsRequired();
            builder.Property(e => e.RestTime).IsRequired();
            builder.Property(e => e.BodyPart).HasMaxLength(100);
            builder.Property(e => e.VideoUrl).HasMaxLength(300);
            builder.Property(e => e.ImageUrl).HasMaxLength(300);
            builder.Property(e => e.Steps).HasMaxLength(1000);
            builder.Property(e => e.Contraindications).HasMaxLength(500);
            builder.Property(e => e.SafetyTips).HasMaxLength(500);
            builder.Property(e => e.CommonMistakes).HasMaxLength(500);
            builder.Property(e => e.IndicatedFor).HasMaxLength(500);
            builder.Property(e => e.CaloriesBurnedEstimate).HasColumnType("float");

            builder.HasOne(e => e.Instructor)
                   .WithMany()
                   .HasForeignKey(e => e.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Level)
                   .WithMany()
                   .HasForeignKey(e => e.LevelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
