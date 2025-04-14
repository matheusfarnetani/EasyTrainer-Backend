using Application.Services.Interfaces;
using Domain.RepositoryInterfaces;

namespace Application.Services.Implementations
{
    public class DeleteValidationService : IDeletionValidationService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public DeleteValidationService(
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository)
        {
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<bool> CanDeleteTypeAsync(int typeId)
        {
            var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId);
            var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId);
            var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId);

            return !workouts.Any() && !routines.Any() && !exercises.Any();
        }

        public async Task<bool> CanDeleteModalityAsync(int modalityId)
        {
            var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId);
            var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId);
            var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId);

            return !workouts.Any() && !routines.Any() && !exercises.Any();
        }

        public async Task<bool> CanDeleteHashtagAsync(int hashtagId)
        {
            var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId);
            var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId);
            var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId);

            return !workouts.Any() && !routines.Any() && !exercises.Any();
        }
    }
}
