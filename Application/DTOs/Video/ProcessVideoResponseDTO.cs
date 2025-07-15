namespace Application.DTOs.Video
{
    public class ProcessVideoResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
        public string? Message { get; set; }
    }
}
