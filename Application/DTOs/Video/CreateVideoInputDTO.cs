using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Video
{
    public class CreateVideoInputDTO
    {
        public IFormFile File { get; set; } = null!;
        public string? OptionalFields { get; set; }
    }
}
