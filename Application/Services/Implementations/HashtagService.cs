using Application.DTOs;
using Application.DTOs.Hashtag;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class HashtagService : GenericService<Hashtag, CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>, IHashtagService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateHashtagInputDTO> _createValidator;
        private readonly IValidator<UpdateHashtagInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public HashtagService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateHashtagInputDTO> createValidator,
            IValidator<UpdateHashtagInputDTO> updateValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        // General
        public override async Task<ServiceResponseDTO<HashtagOutputDTO>> CreateAsync(CreateHashtagInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Hashtag>(dto);
            await _unitOfWork.Hashtags.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<HashtagOutputDTO>.CreateSuccess(_mapper.Map<HashtagOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<HashtagOutputDTO>> UpdateAsync(UpdateHashtagInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var hashtag = await _unitOfWork.Hashtags.GetByIdAsync(dto.Id);
            if (hashtag == null)
                return ServiceResponseDTO<HashtagOutputDTO>.CreateFailure("Hashtag not found.");

            if (dto.Name != null) hashtag.Name = dto.Name;

            await _unitOfWork.Hashtags.UpdateAsync(hashtag);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<HashtagOutputDTO>.CreateSuccess(_mapper.Map<HashtagOutputDTO>(hashtag));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var hashtag = await _unitOfWork.Hashtags.GetByIdAsync(id);
            if (hashtag == null)
                return ServiceResponseDTO<bool>.CreateFailure("Hashtag not found.");

            await _unitOfWork.Hashtags.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }
    }
}