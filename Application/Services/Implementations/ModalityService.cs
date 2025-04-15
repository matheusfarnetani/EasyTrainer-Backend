using Application.DTOs;
using Application.DTOs.Modality;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;

namespace Application.Services.Implementations
{
    public class ModalityService : GenericService<Modality, CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>, IModalityService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public ModalityService(
            IModalityRepository modalityRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IMapper mapper)
            : base(modalityRepository, mapper)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId, instructorId);

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

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId, instructorId);

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

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId, instructorId);

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
