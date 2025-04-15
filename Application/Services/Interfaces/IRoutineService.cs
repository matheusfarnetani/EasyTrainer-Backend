using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;

namespace Application.Services.Interfaces
{
    public interface IRoutineService : IGenericInstructorOwnedService<CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>
    {
        Task AddGoalToRoutineAsync(int routineId, int goalId, int instructorId);
        Task RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId);

        Task AddTypeToRoutineAsync(int routineId, int typeId, int instructorId);
        Task RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId);

        Task AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId);
        Task RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId);

        Task AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId);
        Task RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId);

        Task AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId);
        Task RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId);

        Task<InstructorOutputDTO> GetInstructorByRoutineIdAsync(int routineId);
        Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
    }
}
