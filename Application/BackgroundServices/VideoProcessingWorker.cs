using Application.DTOs.Video;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Application.BackgroundServices
{
    public class VideoProcessingWorker(
        ILogger<VideoProcessingWorker> logger,
        IHttpClientFactory httpClientFactory,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<VideoProcessingWorker> _logger = logger;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("VideoProcessingWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var videoService = scope.ServiceProvider.GetRequiredService<IVideoService>();

                    // Buscar vídeos pendentes
                    var pendingResponse = await videoService.GetPendingVideosAsync();
                    if (!pendingResponse.Success || pendingResponse.Data == null)
                    {
                        _logger.LogWarning("No pending videos found or error fetching videos.");
                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                        continue;
                    }

                    foreach (var video in pendingResponse.Data)
                    {
                        _logger.LogInformation($"Processing video {video.Id}...");

                        // Processamento no Python API
                        var processResult = await ProcessVideoAsync(video);
                        if (processResult == null)
                        {
                            _logger.LogError($"Processing failed for video {video.Id}.");
                            continue;
                        }

                        using var innerScope = _scopeFactory.CreateScope();
                        var innerVideoService = innerScope.ServiceProvider.GetRequiredService<IVideoService>();

                        var updateDto = new UpdateVideoInputDTO
                        {
                            Id = video.Id,
                            Status = processResult.Status == "success" ? 2 : 3,
                            FileUrl = processResult.FileUrl,
                            ErrorMessage = processResult.Message,
                            ProcessedAt = DateTime.UtcNow
                        };

                        await innerVideoService.UpdateAsync(updateDto, systemUserId: -1);
                        _logger.LogInformation($"Updated video {video.Id} status to {updateDto.Status}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing videos.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("VideoProcessingWorker stopped.");
        }

        private async Task<ProcessVideoResponseDTO?> ProcessVideoAsync(VideoOutputDTO video)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var apiUrl = _configuration["PythonAPI:Endpoint"] ?? "http://localhost:8000/process-video";

                var backendBaseUrl = _configuration["BackendAPI:BaseUrl"];

                var requestPayload = new
                {
                    input_url = video.FileUrl,
                    output_public_id = $"processed/{video.UserId}/{Guid.NewGuid()}",
                    video_id = video.Id,
                    backend_base_url = backendBaseUrl
                };

                var json = JsonConvert.SerializeObject(requestPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Python API returned error: {response.StatusCode}");
                    return new ProcessVideoResponseDTO
                    {
                        Status = "error",
                        Message = $"Python API error: {response.StatusCode}"
                    };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProcessVideoResponseDTO>(responseString);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Python API for video processing.");
                return null;
            }
        }
    }
}
