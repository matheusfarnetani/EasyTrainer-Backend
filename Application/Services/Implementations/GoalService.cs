using Application.DTOs;
using Application.DTOs.Goal;
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
    public class GoalService : GenericService<Goal, CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>, IGoalService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateGoalInputDTO> _createValidator;
        private readonly IValidator<UpdateGoalInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public GoalService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateGoalInputDTO> createValidator,
            IValidator<UpdateGoalInputDTO> updateValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _goalIdValidator = goalIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        // General
        public override async Task<ServiceResponseDTO<GoalOutputDTO>> CreateAsync(CreateGoalInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Goal>(dto);
            await _unitOfWork.Goals.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<GoalOutputDTO>.CreateSuccess(_mapper.Map<GoalOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<GoalOutputDTO>> UpdateAsync(UpdateGoalInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var goal = await _unitOfWork.Goals.GetByIdAsync(dto.Id);
            if (goal == null)
                return ServiceResponseDTO<GoalOutputDTO>.CreateFailure("Goal not found.");

            if (dto.Name != null) goal.Name = dto.Name;
            if (dto.Description != null) goal.Description = dto.Description;

            await _unitOfWork.Goals.UpdateAsync(goal);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<GoalOutputDTO>.CreateSuccess(_mapper.Map<GoalOutputDTO>(goal));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var goal = await _unitOfWork.Goals.GetByIdAsync(id);
            if (goal == null)
                return ServiceResponseDTO<bool>.CreateFailure("Goal not found.");

            await _unitOfWork.Goals.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByGoalIdAsync(goalId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }
    }
}