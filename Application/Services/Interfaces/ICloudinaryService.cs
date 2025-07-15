namespace Application.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadVideoAsync(Stream fileStream, string publicId);
        Task DeleteVideoAsync(string publicId);
    }
}
