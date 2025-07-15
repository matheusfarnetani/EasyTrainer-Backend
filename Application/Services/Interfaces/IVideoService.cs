using Application.DTOs;
using Application.DTOs.Video;

namespace Application.Services.Interfaces
{
    public interface IVideoService : IGenericService<CreateVideoInputDTO, UpdateVideoInputDTO, VideoOutputDTO>
    {
        Task<ServiceResponseDTO<VideoOutputDTO>> UpdateAsync(UpdateVideoInputDTO dto, int? systemUserId = null);

        Task<ServiceResponseDTO<IEnumerable<VideoOutputDTO>>> GetPendingVideosAsync();
        Task<ServiceResponseDTO<PaginationResponseDTO<VideoOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);
    }
}
