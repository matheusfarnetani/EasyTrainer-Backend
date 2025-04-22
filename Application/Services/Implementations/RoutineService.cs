using Application.DTOs;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;
using Application.Validators.Level;
using Application.Validators.Workout;

namespace Application.Services.Implementations
{
    public class RoutineService : GenericInstructorOwnedService<Routine, CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>, IRoutineService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateRoutineInputDTO> _createValidator;
        private readonly IValidator<UpdateRoutineInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator;

        public RoutineService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateRoutineInputDTO> createValidator,
            IValidator<UpdateRoutineInputDTO> updateValidator,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator,
            IValidator<IdInputDTO> levelIdValidator,
            IValidator<IdInputDTO> workoutIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _routineIdValidator = routineIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _typeIdValidator = typeIdValidator;
            _modalityIdValidator = modalityIdValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _exerciseIdValidator = exerciseIdValidator;
            _levelIdValidator = levelIdValidator;
            _workoutIdValidator = workoutIdValidator;
        }

        protected override void SetInstructorId(Routine entity, int instructorId)
        {
            if (entity.InstructorId != 0 && entity.InstructorId != instructorId)
                throw new UnauthorizedAccessException("Cannot overwrite instructorId of an existing routine.");
            entity.InstructorId = instructorId;
        }

        protected override bool CheckInstructorOwnership(Routine entity, int instructorId)
        {
            return entity.InstructorId == instructorId;
        }

        private void EnsureInstructorOwnership(Routine entity, int instructorId)
        {
            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedAccessException("This routine does not belong to the specified instructor.");
        }

        private async Task<Routine> GetOwnedRoutineOrThrowAsync(int routineId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routine = await _unitOfWork.Routines.GetByIdAsync(routineId);
            if (routine is null)
                throw new EntityNotFoundException(nameof(Routine), routineId);

            EnsureInstructorOwnership(routine, instructorId);
            return routine;
        }

        public override async Task<ServiceResponseDTO<RoutineOutputDTO>> CreateAsync(CreateRoutineInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Routine>(dto);
            SetInstructorId(entity, instructorId);

            await _unitOfWork.Routines.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<RoutineOutputDTO>.CreateSuccess(_mapper.Map<RoutineOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<RoutineOutputDTO>> UpdateAsync(UpdateRoutineInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var entity = await GetOwnedRoutineOrThrowAsync(dto.Id, instructorId);

            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.ImageUrl != null) entity.ImageUrl = dto.ImageUrl;
            if (dto.Duration.HasValue) entity.Duration = dto.Duration.Value;
            if (dto.LevelId.HasValue) entity.LevelId = dto.LevelId.Value;

            await _unitOfWork.Routines.UpdateAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<RoutineOutputDTO>.CreateSuccess(_mapper.Map<RoutineOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId)
        {
            var entity = await GetOwnedRoutineOrThrowAsync(id, instructorId);
            await _unitOfWork.Routines.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // --- Relationships ---
        public async Task<ServiceResponseDTO<bool>> AddGoalToRoutineAsync(int routineId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.AddGoalToRoutineAsync(routineId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.RemoveGoalFromRoutineAsync(routineId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddTypeToRoutineAsync(int routineId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.AddTypeToRoutineAsync(routineId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.RemoveTypeFromRoutineAsync(routineId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.AddModalityToRoutineAsync(routineId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.RemoveModalityFromRoutineAsync(routineId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.AddHashtagToRoutineAsync(routineId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.RemoveHashtagFromRoutineAsync(routineId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.AddExerciseToRoutineAsync(routineId, exerciseId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await GetOwnedRoutineOrThrowAsync(routineId, instructorId);
            await _unitOfWork.Routines.RemoveExerciseFromRoutineAsync(routineId, exerciseId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByRoutineIdAsync(int routineId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });

            var instructor = await _unitOfWork.Routines.GetInstructorByRoutineIdAsync(routineId);
            if (instructor is null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found for this routine.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByRoutineIdAsync(routineId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByWorkoutIdAsync(workoutId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByExerciseIdAsync(exerciseId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

    }
}
