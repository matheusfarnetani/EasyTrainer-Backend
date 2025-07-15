using Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("user_videos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Filename)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("filename");

            builder.Property(v => v.FileUrl)
                   .IsRequired()
                   .HasMaxLength(512)
                   .HasColumnName("file_url");

            builder.Property(v => v.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(v => v.UploadedAt)
                   .HasColumnName("uploaded_at");

            builder.Property(v => v.ProcessedAt)
                   .HasColumnName("processed_at");

            builder.Property(v => v.ErrorMessage)
                   .HasColumnName("error_message");

            builder.Property(v => v.UserId)
                   .HasColumnName("user_id");

            builder.HasOne(v => v.User)
                   .WithMany()
                   .HasForeignKey(v => v.UserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("fk_user_videos_user");
        }
    }
}
