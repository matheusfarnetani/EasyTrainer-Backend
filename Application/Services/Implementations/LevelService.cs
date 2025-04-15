using Application.DTOs;
using Application.DTOs.Level;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;

namespace Application.Services.Implementations
{
    public class LevelService : GenericService<Level, CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>, ILevelService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public LevelService(
            ILevelRepository levelRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IMapper mapper)
            : base(levelRepository, mapper)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            var workouts = await _workoutRepository.GetWorkoutsByLevelIdAsync(levelId, instructorId);

            var paginated = workouts
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<WorkoutOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = workouts.Count(),
                Data = _mapper.Map<List<WorkoutOutputDTO>>(paginated)
            };
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            var routines = await _routineRepository.GetRoutinesByLevelIdAsync(levelId, instructorId);

            var paginated = routines
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<RoutineOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = routines.Count(),
                Data = _mapper.Map<List<RoutineOutputDTO>>(paginated)
            };
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination)
        {
            var exercises = await _exerciseRepository.GetExercisesByLevelIdAsync(levelId, instructorId);

            var paginated = exercises
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<ExerciseOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = exercises.Count(),
                Data = _mapper.Map<List<ExerciseOutputDTO>>(paginated)
            };
        }
    }
}
