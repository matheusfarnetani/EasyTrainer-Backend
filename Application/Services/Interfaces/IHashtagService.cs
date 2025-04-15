using Application.DTOs;
using Application.DTOs.Hashtag;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Services.Interfaces;

public interface IHashtagService : IGenericService<CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
}
