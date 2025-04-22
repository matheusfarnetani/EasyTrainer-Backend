using Application.Services.Interfaces;
using Domain.Infrastructure.RepositoriesInterfaces;

public class DeletionValidationService : IDeletionValidationService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IRoutineRepository _routineRepository;
    private readonly IExerciseRepository _exerciseRepository;

    public DeletionValidationService(
        IWorkoutRepository workoutRepository,
        IRoutineRepository routineRepository,
        IExerciseRepository exerciseRepository)
    {
        _workoutRepository = workoutRepository;
        _routineRepository = routineRepository;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<bool> CanDeleteTypeAsync(int typeId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId, instructorId);
        var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteModalityAsync(int modalityId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId, instructorId);
        var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteHashtagAsync(int hashtagId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);
        var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteGoalAsync(int goalId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByGoalIdAsync(goalId, instructorId);
        var routines = await _routineRepository.GetRoutinesByGoalIdAsync(goalId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByGoalIdAsync(goalId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }
}
