using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface IWorkoutService : IGenericInstructorOwnedService<CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>
    {
        // Relationship management: Add/Remove
        Task<ServiceResponseDTO<bool>> AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId);

        // Retrievals
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByWorkoutIdAsync(int workoutId);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
    }
}
