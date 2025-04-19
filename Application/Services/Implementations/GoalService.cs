using Application.DTOs;
using Application.DTOs.Goal;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class GoalService : GenericService<Goal, CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>, IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateGoalInputDTO> _createValidator;
        private readonly IValidator<UpdateGoalInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public GoalService(
            IGoalRepository goalRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateGoalInputDTO> createValidator,
            IValidator<UpdateGoalInputDTO> updateValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(goalRepository, mapper)
        {
            _goalRepository = goalRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _goalIdValidator = goalIdValidator;
        }

        public override async Task<GoalOutputDTO> CreateAsync(CreateGoalInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Goal>(dto);
            await _goalRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GoalOutputDTO>(entity);
        }

        public override async Task<GoalOutputDTO> UpdateAsync(UpdateGoalInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var goal = await _goalRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) goal.Name = dto.Name;
            if (dto.Description != null) goal.Description = dto.Description;

            await _goalRepository.UpdateAsync(goal);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<GoalOutputDTO>(goal);
        }

        public override async Task DeleteAsync(int id)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _goalRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _workoutRepository.GetWorkoutsByGoalIdAsync(goalId, instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByGoalIdAsync(goalId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByGoalIdAsync(goalId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
