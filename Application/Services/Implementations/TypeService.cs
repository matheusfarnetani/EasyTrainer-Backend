using Application.DTOs;
using Application.DTOs.TrainingType;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;

namespace Application.Services.Implementations
{
    public class TypeService : GenericService<TrainingType, CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>, ITypeService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public TypeService(
            ITypeRepository typeRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IMapper mapper)
            : base(typeRepository, mapper)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId, instructorId);

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

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId, instructorId);

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

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId, instructorId);

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
