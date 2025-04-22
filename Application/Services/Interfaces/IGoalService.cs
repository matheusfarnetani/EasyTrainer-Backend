using Application.DTOs;
using Application.DTOs.Goal;
using Application.DTOs.Exercise;
using Application.DTOs.Routine;
using Application.DTOs.Workout;

namespace Application.Services.Interfaces
{
    public interface IGoalService : IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>
    {
        // Workout
        Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);

        // Routine
        Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);

        // Exercise
        Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    }
}