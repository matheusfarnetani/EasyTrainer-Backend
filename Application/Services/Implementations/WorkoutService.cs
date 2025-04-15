using Application.DTOs;
using Application.DTOs.Instructor;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Exceptions;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using Domain.SystemInterfaces;
using AutoMapper;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class WorkoutService : GenericInstructorOwnedService<Workout, CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>, IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateWorkoutInputDTO> _createValidator;
        private readonly IValidator<UpdateWorkoutInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;

        public WorkoutService(
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateWorkoutInputDTO> createValidator,
            IValidator<UpdateWorkoutInputDTO> updateValidator,
            IValidator<IdInputDTO> workoutIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator)
            : base(workoutRepository, mapper)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _workoutIdValidator = workoutIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _typeIdValidator = typeIdValidator;
            _modalityIdValidator = modalityIdValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _routineIdValidator = routineIdValidator;
            _exerciseIdValidator = exerciseIdValidator;
        }

        public override async Task<WorkoutOutputDTO> CreateAsync(CreateWorkoutInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Workout>(dto);
            SetInstructorId(entity, instructorId);

            await _workoutRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<WorkoutOutputDTO>(entity);
        }

        public override async Task<WorkoutOutputDTO> UpdateAsync(UpdateWorkoutInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workout = await _workoutRepository.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException(nameof(Workout), dto.Id);

            EnsureInstructorOwnership(workout, instructorId);

            if (dto.Name != null) workout.Name = dto.Name;
            if (dto.Description != null) workout.Description = dto.Description;

            await _workoutRepository.UpdateAsync(workout);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<WorkoutOutputDTO>(workout);
        }

        public override async Task DeleteAsync(int id, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workout = await _workoutRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Workout), id);

            EnsureInstructorOwnership(workout, instructorId);

            await _workoutRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddGoalToWorkoutAsync(workoutId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveGoalFromWorkoutAsync(workoutId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddTypeToWorkoutAsync(workoutId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveTypeFromWorkoutAsync(workoutId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddModalityToWorkoutAsync(workoutId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveModalityFromWorkoutAsync(workoutId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddHashtagToWorkoutAsync(workoutId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveHashtagFromWorkoutAsync(workoutId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddRoutineToWorkoutAsync(workoutId, routineId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveRoutineFromWorkoutAsync(workoutId, routineId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.AddExerciseToWorkoutAsync(workoutId, exerciseId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _workoutRepository.RemoveExerciseFromWorkoutAsync(workoutId, exerciseId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<InstructorOutputDTO> GetInstructorByWorkoutIdAsync(int workoutId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var instructor = await _workoutRepository.GetInstructorByWorkoutIdAsync(workoutId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByWorkoutIdAsync(workoutId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByWorkoutIdAsync(workoutId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }

        protected override bool CheckInstructorOwnership(Workout entity, int instructorId)
        {
            return entity.InstructorId == instructorId;
        }

        private void EnsureInstructorOwnership(Workout entity, int instructorId)
        {
            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedAccessException("This workout does not belong to the specified instructor.");
        }
        protected override void SetInstructorId(Workout entity, int instructorId)
        {
            if (entity.InstructorId != 0 && entity.InstructorId != instructorId)
                throw new UnauthorizedAccessException("Cannot overwrite instructorId of an existing workout.");

            entity.InstructorId = instructorId;
        }

    }
}
