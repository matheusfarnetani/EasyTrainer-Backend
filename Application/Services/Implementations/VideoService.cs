using Application.DTOs;
using Application.DTOs.Video;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using Domain.API.Interfaces;
using FluentValidation;
using Application.Helpers;

namespace Application.Services.Implementations
{
    public class VideoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateVideoInputDTO> createValidator,
        IValidator<UpdateVideoInputDTO> updateValidator,
        IValidator<IdInputDTO> idValidator,
        ICloudinaryService cloudinaryService,
        ICurrentUserContext currentUserContext
    ) : GenericService<Video, CreateVideoInputDTO, UpdateVideoInputDTO, VideoOutputDTO>(unitOfWork, mapper), IVideoService
    {
        private new readonly IMapper _mapper = mapper;
        private new readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidator<CreateVideoInputDTO> _createValidator = createValidator;
        private readonly IValidator<UpdateVideoInputDTO> _updateValidator = updateValidator;
        private readonly IValidator<IdInputDTO> _idValidator = idValidator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly ICurrentUserContext _currentUserContext = currentUserContext;

        public override async Task<ServiceResponseDTO<VideoOutputDTO>> CreateAsync(CreateVideoInputDTO dto)
        {
            var allowedExtensions = new[] { ".mp4", ".mov", ".avi", ".webm", ".mkv" };
            var fileExtension = Path.GetExtension(dto.File.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return ServiceResponseDTO<VideoOutputDTO>.CreateFailure($"Unsupported file type: {fileExtension}. Allowed: {string.Join(", ", allowedExtensions)}");
            }

            await _createValidator.ValidateAndThrowAsync(dto);

            var userId = _currentUserContext.Id;
            var publicId = $"original/{userId}/{Guid.NewGuid()}";
            var secureUrl = await _cloudinaryService.UploadVideoAsync(dto.File.OpenReadStream(), publicId);

            var video = new Video
            {
                UserId = userId,
                Filename = dto.File.FileName,
                FileUrl = secureUrl,
                Status = 0,
                UploadedAt = DateTime.UtcNow
            };

            await _unitOfWork.Videos.AddAsync(video);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<VideoOutputDTO>(video);
            return ServiceResponseDTO<VideoOutputDTO>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<VideoOutputDTO>> UpdateAsync(UpdateVideoInputDTO dto, int? systemUserId = null)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var video = await _unitOfWork.Videos.GetByIdAsync(dto.Id);
            if (video == null)
                return ServiceResponseDTO<VideoOutputDTO>.CreateFailure("Video not found.");

            if (!string.IsNullOrEmpty(dto.Filename)) video.Filename = dto.Filename;
            if (dto.Status.HasValue) video.Status = dto.Status.Value;
            if (!string.IsNullOrEmpty(dto.ErrorMessage)) video.ErrorMessage = dto.ErrorMessage;
            if (!string.IsNullOrEmpty(dto.FileUrl)) video.FileUrl = dto.FileUrl;
            if (dto.ProcessedAt.HasValue) video.ProcessedAt = dto.ProcessedAt;

            await _unitOfWork.Videos.UpdateAsync(video);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<VideoOutputDTO>.CreateSuccess(_mapper.Map<VideoOutputDTO>(video));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var video = await _unitOfWork.Videos.GetByIdAsync(id);
            if (video == null)
                return ServiceResponseDTO<bool>.CreateFailure("Video not found.");

            if (!string.IsNullOrEmpty(video.FileUrl))
            {
                var publicId = GetPublicIdFromUrl(video.FileUrl);
                await _cloudinaryService.DeleteVideoAsync(publicId);
            }

            await _unitOfWork.Videos.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public override async Task<ServiceResponseDTO<PaginationResponseDTO<VideoOutputDTO>>> GetAllAsync(PaginationRequestDTO pagination)
        {
            var videos = await _unitOfWork.Videos.GetFreshVideosAsync();
            var result = PaginationHelper.Paginate<Video, VideoOutputDTO>(videos, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<VideoOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<IEnumerable<VideoOutputDTO>>> GetPendingVideosAsync()
        {
            var videos = await _unitOfWork.Videos.GetPendingVideosAsync();
            return ServiceResponseDTO<IEnumerable<VideoOutputDTO>>.CreateSuccess(_mapper.Map<IEnumerable<VideoOutputDTO>>(videos));
        }

        private static string GetPublicIdFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var parts = uri.AbsolutePath.Split('/');
            var index = Array.FindIndex(parts, x => x == "upload");
            if (index == -1 || index + 1 >= parts.Length) throw new InvalidOperationException("Invalid Cloudinary URL format.");
            var publicIdParts = parts.Skip(index + 1).ToArray();
            var lastPart = publicIdParts.Last();
            var publicId = string.Join("/", publicIdParts).Replace($"/{lastPart}", "").Replace(".mp4", "");
            return publicId;
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<VideoOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            var videos = await _unitOfWork.Videos.GetByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Video, VideoOutputDTO>(videos, pagination, _mapper);
            return ServiceResponseDTO<PaginationResponseDTO<VideoOutputDTO>>.CreateSuccess(result);
        }
    }
}
