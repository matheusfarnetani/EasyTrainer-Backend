using Application.DTOs;
using Application.DTOs.Goal;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Services.Interfaces;

public interface IGoalService : IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
}
