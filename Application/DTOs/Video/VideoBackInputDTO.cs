namespace Application.DTOs.Video
{
    public class VideoBackInputDTO
    {
        public string InputUrl { get; set; } = string.Empty;
        public string OutputPublicId { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
