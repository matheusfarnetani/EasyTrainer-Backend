using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class ExerciseService(
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
        IValidator<IdInputDTO> variationIdValidator,
        IValidator<IdInputDTO> levelIdValidator,
        IValidator<IdInputDTO> workoutIdValidator,
        IValidator<IdInputDTO> routineIdValidator) : GenericInstructorOwnedService<Exercise, CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO>(unitOfWork, mapper), IExerciseService
    {
        private new readonly IUnitOfWork _unitOfWork = unitOfWork;
        private new readonly IMapper _mapper = mapper;

        private readonly IValidator<CreateExerciseInputDTO> _createValidator = createValidator;
        private readonly IValidator<UpdateExerciseInputDTO> _updateValidator = updateValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator = exerciseIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator = instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator = goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator = typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator = modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator = hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _variationIdValidator = variationIdValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator = levelIdValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator = workoutIdValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator = routineIdValidator;

        #region CRUD

        public override async Task<ServiceResponseDTO<ExerciseOutputDTO>> CreateAsync(CreateExerciseInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Exercise>(dto);
            SetInstructorId(entity, instructorId);

            await _unitOfWork.Exercises.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<ExerciseOutputDTO>.CreateSuccess(_mapper.Map<ExerciseOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<ExerciseOutputDTO>> UpdateAsync(UpdateExerciseInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var entity = await GetOwnedExerciseOrThrowAsync(dto.Id, instructorId);

            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.Equipment != null) entity.Equipment = dto.Equipment;
            if (dto.BodyPart != null) entity.BodyPart = dto.BodyPart;
            if (dto.VideoUrl != null) entity.VideoUrl = dto.VideoUrl;
            if (dto.ImageUrl != null) entity.ImageUrl = dto.ImageUrl;
            if (dto.Steps != null) entity.Steps = dto.Steps;
            if (dto.Contraindications != null) entity.Contraindications = dto.Contraindications;
            if (dto.SafetyTips != null) entity.SafetyTips = dto.SafetyTips;
            if (dto.CommonMistakes != null) entity.CommonMistakes = dto.CommonMistakes;
            if (dto.IndicatedFor != null) entity.IndicatedFor = dto.IndicatedFor;
            if (dto.Duration.HasValue) entity.Duration = dto.Duration.Value;
            if (dto.Repetition.HasValue) entity.Repetition = dto.Repetition.Value;
            if (dto.Sets.HasValue) entity.Sets = dto.Sets.Value;
            if (dto.RestTime.HasValue) entity.RestTime = dto.RestTime.Value;
            if (dto.CaloriesBurnedEstimate.HasValue) entity.CaloriesBurnedEstimate = dto.CaloriesBurnedEstimate.Value;
            if (dto.LevelId.HasValue) entity.LevelId = dto.LevelId.Value;

            await _unitOfWork.Exercises.UpdateAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<ExerciseOutputDTO>.CreateSuccess(_mapper.Map<ExerciseOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId)
        {
            var entity = await GetOwnedExerciseOrThrowAsync(id, instructorId);
            await _unitOfWork.Exercises.DeleteByIdAsync(entity.Id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        #endregion

        #region Ownership & Validation Helpers

        private async Task<Exercise> GetOwnedExerciseOrThrowAsync(int exerciseId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = await _unitOfWork.Exercises.GetByIdAsync(exerciseId)
                ?? throw new EntityNotFoundException(nameof(Exercise), exerciseId);

            EnsureInstructorOwnership(entity, instructorId);
            return entity;
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

        #endregion

        #region Relationship Management

        public async Task<ServiceResponseDTO<bool>> AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.AddGoalToExerciseAsync(exerciseId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.RemoveGoalFromExerciseAsync(exerciseId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.AddTypeToExerciseAsync(exerciseId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.RemoveTypeFromExerciseAsync(exerciseId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.AddModalityToExerciseAsync(exerciseId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.RemoveModalityFromExerciseAsync(exerciseId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.AddHashtagToExerciseAsync(exerciseId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.RemoveHashtagFromExerciseAsync(exerciseId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            await _variationIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = variationId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.AddVariationToExerciseAsync(exerciseId, variationId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId)
        {
            await _variationIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = variationId });
            await GetOwnedExerciseOrThrowAsync(exerciseId, instructorId);

            await _unitOfWork.Exercises.RemoveVariationFromExerciseAsync(exerciseId, variationId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        #endregion

        #region Data Retrieval

        protected override async Task<IEnumerable<Exercise>> GetEntitiesWithIncludes(int instructorId)
        {
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });
            return await _unitOfWork.Exercises.GetExercisesByInstructorIdAsync(instructorId);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByExerciseIdAsync(int exerciseId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });

            var instructor = await _unitOfWork.Exercises.GetInstructorByExerciseIdAsync(exerciseId)
                ?? throw new EntityNotFoundException(nameof(Instructor), exerciseId);

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByRoutineIdAsync(routineId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetExercisesByWorkoutIdAsync(workoutId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetVariationsByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var items = await _unitOfWork.Exercises.GetVariationsByExerciseAsync(exerciseId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(items, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        #endregion
    }
}
