namespace Application.DTOs.Video
{
    public class VideoOutputDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
