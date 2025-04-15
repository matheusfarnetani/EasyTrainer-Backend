using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;

namespace Application.Services.Interfaces
{
    public interface IExerciseService : IGenericInstructorOwnedService<CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO>
    {
        Task AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId);
        Task RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId);

        Task AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId);
        Task RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId);

        Task AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId);
        Task RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId);

        Task AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId);
        Task RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId);

        Task AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId);
        Task RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId);

        Task<InstructorOutputDTO> GetInstructorByExerciseIdAsync(int exerciseId);
        Task<PaginationResponseDTO<ExerciseOutputDTO>> GetVariationsByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
    }
}
