using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;

namespace Application.Services.Interfaces
{
    public interface IExerciseService : IGenericInstructorOwnedService<CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO>
    {
        // Relationship management
        Task<ServiceResponseDTO<bool>> AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId);

        Task<ServiceResponseDTO<bool>> AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId);
        Task<ServiceResponseDTO<bool>> RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId);

        // Direct retrieval
        Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByExerciseIdAsync(int exerciseId);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetVariationsByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);

        // Data retrieval by relations
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
    }
}
