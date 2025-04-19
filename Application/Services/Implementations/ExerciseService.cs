using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class ExerciseService : GenericInstructorOwnedService<Exercise, CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO>, IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateExerciseInputDTO> _createValidator;
        private readonly IValidator<UpdateExerciseInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _variationIdValidator;

        public ExerciseService(
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateExerciseInputDTO> createValidator,
            IValidator<UpdateExerciseInputDTO> updateValidator,
            IValidator<IdInputDTO> exerciseIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> variationIdValidator)
            : base(exerciseRepository, mapper)
        {
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _exerciseIdValidator = exerciseIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _typeIdValidator = typeIdValidator;
            _modalityIdValidator = modalityIdValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _variationIdValidator = variationIdValidator;
        }

        public override async Task<ExerciseOutputDTO> CreateAsync(CreateExerciseInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Exercise>(dto);
            SetInstructorId(entity, instructorId);

            await _exerciseRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ExerciseOutputDTO>(entity);
        }

        public override async Task<ExerciseOutputDTO> UpdateAsync(UpdateExerciseInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercise = await _exerciseRepository.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException(nameof(Exercise), dto.Id);

            EnsureInstructorOwnership(exercise, instructorId);

            if (dto.Name != null) exercise.Name = dto.Name;
            if (dto.Description != null) exercise.Description = dto.Description;

            await _exerciseRepository.UpdateAsync(exercise);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ExerciseOutputDTO>(exercise);
        }

        public override async Task DeleteAsync(int id, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercise = await _exerciseRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Exercise), id);

            EnsureInstructorOwnership(exercise, instructorId);

            await _exerciseRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.AddGoalToExerciseAsync(exerciseId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.RemoveGoalFromExerciseAsync(exerciseId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.AddTypeToExerciseAsync(exerciseId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.RemoveTypeFromExerciseAsync(exerciseId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.AddModalityToExerciseAsync(exerciseId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.RemoveModalityFromExerciseAsync(exerciseId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.AddHashtagToExerciseAsync(exerciseId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.RemoveHashtagFromExerciseAsync(exerciseId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _variationIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = variationId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.AddVariationToExerciseAsync(exerciseId, variationId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _variationIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = variationId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _exerciseRepository.RemoveVariationFromExerciseAsync(exerciseId, variationId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<InstructorOutputDTO> GetInstructorByExerciseIdAsync(int exerciseId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });

            var instructor = await _exerciseRepository.GetInstructorByExerciseIdAsync(exerciseId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetVariationsByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var variations = await _exerciseRepository.GetVariationsByExerciseAsync(exerciseId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(variations, pagination, _mapper);
        }

        protected override void SetInstructorId(Exercise entity, int instructorId)
        {
            if (entity.InstructorId != 0 && entity.InstructorId != instructorId)
                throw new UnauthorizedAccessException("Cannot overwrite instructorId of an existing exercise.");

            entity.InstructorId = instructorId;
        }

        protected override bool CheckInstructorOwnership(Exercise entity, int instructorId)
        {
            return entity.InstructorId == instructorId;
        }

        private void EnsureInstructorOwnership(Exercise entity, int instructorId)
        {
            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedAccessException("This exercise does not belong to the specified instructor.");
        }
    }
}
