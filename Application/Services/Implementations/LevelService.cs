using Application.DTOs;
using Application.DTOs.Level;
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
    public class LevelService : GenericService<Level, CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>, ILevelService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateLevelInputDTO> _createValidator;
        private readonly IValidator<UpdateLevelInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public LevelService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateLevelInputDTO> createValidator,
            IValidator<UpdateLevelInputDTO> updateValidator,
            IValidator<IdInputDTO> levelIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _levelIdValidator = levelIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        // General
        public override async Task<ServiceResponseDTO<LevelOutputDTO>> CreateAsync(CreateLevelInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Level>(dto);
            await _unitOfWork.Levels.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<LevelOutputDTO>.CreateSuccess(_mapper.Map<LevelOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<LevelOutputDTO>> UpdateAsync(UpdateLevelInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var level = await _unitOfWork.Levels.GetByIdAsync(dto.Id);
            if (level == null)
                return ServiceResponseDTO<LevelOutputDTO>.CreateFailure("Level not found.");

            if (dto.Name != null) level.Name = dto.Name;
            if (dto.Description != null) level.Description = dto.Description;

            await _unitOfWork.Levels.UpdateAsync(level);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<LevelOutputDTO>.CreateSuccess(_mapper.Map<LevelOutputDTO>(level));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var level = await _unitOfWork.Levels.GetByIdAsync(id);
            if (level == null)
                return ServiceResponseDTO<bool>.CreateFailure("Level not found.");

            await _unitOfWork.Levels.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByLevelIdAsync(levelId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }
    }
}