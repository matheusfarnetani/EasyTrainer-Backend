using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;

namespace Application.Services.Interfaces
{
    public interface IRoutineService : IGenericInstructorOwnedService<CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>
    {
        // Relationship management
        Task<ServiceResponseDTO<bool>> AddGoalToRoutineAsync(int routineId, int goalId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddTypeToRoutineAsync(int routineId, int typeId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId);

        // Data retrieval
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByRoutineIdAsync(int routineId);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);

        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
    }
}
