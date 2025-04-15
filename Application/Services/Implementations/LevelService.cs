using Application.DTOs;
using Application.DTOs.Level;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using Domain.SystemInterfaces;
using AutoMapper;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class LevelService : GenericService<Level, CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>, ILevelService
    {
        private readonly ILevelRepository _levelRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateLevelInputDTO> _createValidator;
        private readonly IValidator<UpdateLevelInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public LevelService(
            ILevelRepository levelRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateLevelInputDTO> createValidator,
            IValidator<UpdateLevelInputDTO> updateValidator,
            IValidator<IdInputDTO> levelIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(levelRepository, mapper)
        {
            _levelRepository = levelRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _levelIdValidator = levelIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        public override async Task<LevelOutputDTO> CreateAsync(CreateLevelInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Level>(dto);
            await _levelRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<LevelOutputDTO>(entity);
        }

        public override async Task<LevelOutputDTO> UpdateAsync(UpdateLevelInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var level = await _levelRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) level.Name = dto.Name;

            await _levelRepository.UpdateAsync(level);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<LevelOutputDTO>(level);
        }

        public override async Task DeleteAsync(int id)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _levelRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _workoutRepository.GetWorkoutsByLevelIdAsync(levelId, instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByLevelIdAsync(levelId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByLevelIdAsync(levelId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
