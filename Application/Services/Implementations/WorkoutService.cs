using Application.DTOs;
using Application.DTOs.Instructor;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Exceptions;
using Application.Helpers;
using Domain.Entities.Main;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.Persistence;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class WorkoutService : GenericInstructorOwnedService<Workout, CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>, IWorkoutService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateWorkoutInputDTO> _createValidator;
        private readonly IValidator<UpdateWorkoutInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _idValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;

        public WorkoutService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateWorkoutInputDTO> createValidator,
            IValidator<UpdateWorkoutInputDTO> updateValidator,
            IValidator<IdInputDTO> idValidator,
            IValidator<IdInputDTO> levelIdValidator,
            IValidator<IdInputDTO> workoutIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _idValidator = idValidator;
            _levelIdValidator = levelIdValidator;
            _workoutIdValidator = workoutIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _typeIdValidator = typeIdValidator;
            _modalityIdValidator = modalityIdValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _routineIdValidator = routineIdValidator;
            _exerciseIdValidator = exerciseIdValidator;
        }

        // Ownership Validation
        protected override bool CheckInstructorOwnership(Workout entity, int instructorId)
        {
            return entity.InstructorId == instructorId;
        }

        // Set InstructorId when creating a workout
        protected override void SetInstructorId(Workout entity, int instructorId)
        {
            if (entity.InstructorId != 0 && entity.InstructorId != instructorId)
                throw new UnauthorizedAccessException("Cannot overwrite instructorId of an existing workout.");

            entity.InstructorId = instructorId;
        }

        // Ensure the instructor owns the workout
        private void EnsureInstructorOwnership(Workout entity, int instructorId)
        {
            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedAccessException("This workout does not belong to the specified instructor.");
        }

        private async Task<Workout> GetOwnedWorkoutOrThrowAsync(int workoutId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = await _unitOfWork.Workouts.GetByIdAsync(workoutId);
            if (entity == null)
                throw new EntityNotFoundException(nameof(Workout), workoutId);

            EnsureInstructorOwnership(entity, instructorId);

            return entity;
        }

        public override async Task<ServiceResponseDTO<WorkoutOutputDTO>> CreateAsync(CreateWorkoutInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Workout>(dto);
            SetInstructorId(entity, instructorId);

            await _unitOfWork.Workouts.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<WorkoutOutputDTO>.CreateSuccess(_mapper.Map<WorkoutOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<WorkoutOutputDTO>> UpdateAsync(UpdateWorkoutInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var entity = await GetOwnedWorkoutOrThrowAsync(dto.Id, instructorId);

            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.ImageUrl != null) entity.ImageUrl = dto.ImageUrl;
            if (dto.Duration != null) entity.Duration = dto.Duration.Value;
            if (dto.NumberOfDays != null) entity.NumberOfDays = dto.NumberOfDays.Value;
            if (dto.Indoor != null) entity.Indoor = dto.Indoor.Value;
            if (dto.LevelId.HasValue) entity.LevelId = dto.LevelId.Value;

            await _unitOfWork.Workouts.UpdateAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<WorkoutOutputDTO>.CreateSuccess(_mapper.Map<WorkoutOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId)
        {
            var entity = await GetOwnedWorkoutOrThrowAsync(id, instructorId);

            await _unitOfWork.Workouts.DeleteByIdAsync(entity.Id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Goal
        public async Task<ServiceResponseDTO<bool>> AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddGoalToWorkoutAsync(workoutId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveGoalFromWorkoutAsync(workoutId, goalId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Type
        public async Task<ServiceResponseDTO<bool>> AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddTypeToWorkoutAsync(workoutId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveTypeFromWorkoutAsync(workoutId, typeId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Modality
        public async Task<ServiceResponseDTO<bool>> AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddModalityToWorkoutAsync(workoutId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveModalityFromWorkoutAsync(workoutId, modalityId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Hashtag
        public async Task<ServiceResponseDTO<bool>> AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddHashtagToWorkoutAsync(workoutId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveHashtagFromWorkoutAsync(workoutId, hashtagId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Routine
        public async Task<ServiceResponseDTO<bool>> AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddRoutineToWorkoutAsync(workoutId, routineId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveRoutineFromWorkoutAsync(workoutId, routineId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Exercise
        public async Task<ServiceResponseDTO<bool>> AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.AddExerciseToWorkoutAsync(workoutId, exerciseId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await GetOwnedWorkoutOrThrowAsync(workoutId, instructorId);

            await _unitOfWork.Workouts.RemoveExerciseFromWorkoutAsync(workoutId, exerciseId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByWorkoutIdAsync(int workoutId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var instructor = await _unitOfWork.Workouts.GetInstructorByWorkoutIdAsync(workoutId);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found for this workout.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByWorkoutIdAsync(workoutId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByWorkoutIdAsync(workoutId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByRoutineIdAsync(routineId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByExerciseIdAsync(exerciseId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }
    }
}
