using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.DTOs;
using Application.Services.Interfaces;

public interface IWorkoutService : IGenericInstructorOwnedService<CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>
{
    Task AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId);
    Task RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId);

    Task AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId);
    Task RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId);

    Task AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId);
    Task RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId);

    Task AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId);
    Task RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId);

    Task AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId);
    Task RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId);

    Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId);
    Task RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId);

    Task<InstructorOutputDTO> GetInstructorByWorkoutIdAsync(int workoutId);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
}