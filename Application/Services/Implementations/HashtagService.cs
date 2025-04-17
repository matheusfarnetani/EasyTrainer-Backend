using Application.DTOs;
using Application.DTOs.Hashtag;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure;

namespace Application.Services.Implementations
{
    public class HashtagService : GenericService<Hashtag, CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>, IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateHashtagInputDTO> _createValidator;
        private readonly IValidator<UpdateHashtagInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public HashtagService(
            IHashtagRepository hashtagRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateHashtagInputDTO> createValidator,
            IValidator<UpdateHashtagInputDTO> updateValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(hashtagRepository, mapper)
        {
            _hashtagRepository = hashtagRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        public override async Task<HashtagOutputDTO> CreateAsync(CreateHashtagInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Hashtag>(dto);
            await _hashtagRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<HashtagOutputDTO>(entity);
        }

        public override async Task<HashtagOutputDTO> UpdateAsync(UpdateHashtagInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var hashtag = await _hashtagRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) hashtag.Name = dto.Name;

            await _hashtagRepository.UpdateAsync(hashtag);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<HashtagOutputDTO>(hashtag);
        }

        public override async Task DeleteAsync(int id)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _hashtagRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
