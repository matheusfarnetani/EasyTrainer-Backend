using Application.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
        {
            _logger = logger;

            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };
        }

        public async Task<string> UploadVideoAsync(Stream fileStream, string publicId)
        {
            try
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription("video.mp4", fileStream),
                    PublicId = publicId
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode != System.Net.HttpStatusCode.OK || result.Error != null)
                {
                    var message = result.Error?.Message ?? "Unknown error";
                    _logger.LogError($"Cloudinary upload failed: {message}");
                    throw new Exception($"Cloudinary upload failed: {message}");
                }

                return result.SecureUrl?.ToString() ?? throw new Exception("SecureUrl not returned from Cloudinary.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading video to Cloudinary.");
                throw;
            }
        }


        public async Task DeleteVideoAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = CloudinaryDotNet.Actions.ResourceType.Video
                };

                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result != "ok")
                {
                    _logger.LogWarning($"Cloudinary deletion returned: {result.Result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting video with publicId {publicId}");
                throw;
            }
        }
    }
}
