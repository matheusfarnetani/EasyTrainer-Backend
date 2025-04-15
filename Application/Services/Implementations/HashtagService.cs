using Application.DTOs;
using Application.DTOs.Hashtag;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using AutoMapper;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;

namespace Application.Services.Implementations
{
    public class HashtagService : GenericService<Hashtag, CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>, IHashtagService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public HashtagService(
            IHashtagRepository hashtagRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IMapper mapper)
            : base(hashtagRepository, mapper)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);

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

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);

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

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination)
        {
            var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId, instructorId);

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
