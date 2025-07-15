namespace Domain.Entities.Main
{
    public class Video
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        public string Filename { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }

        // Navigation
        public User User { get; set; } = null!;
    }
}
