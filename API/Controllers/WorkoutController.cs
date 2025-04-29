using Application.DTOs;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class WorkoutController(IWorkoutService workoutService) : ControllerBase
    {
        private readonly IWorkoutService _workoutService = workoutService;

        // Get a workout by ID (must belong to instructor)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int instructorId)
        {
            var result = await _workoutService.GetByIdAsync(id, instructorId);
            return Ok(result);
        }

        // Get all workouts for instructor (paginated)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetAllAsync(instructorId, pagination);
            return Ok(result);
        }

        // Create a workout for instructor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutInputDTO dto, [FromQuery] int instructorId)
        {
            var result = await _workoutService.CreateAsync(dto, instructorId);
            return Ok(result);
        }

        // Update a workout (must belong to instructor)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkoutInputDTO dto, [FromQuery] int instructorId)
        {
            dto.Id = id;
            var result = await _workoutService.UpdateAsync(dto, instructorId);
            return Ok(result);
        }

        // Delete a workout (must belong to instructor)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int instructorId)
        {
            var result = await _workoutService.DeleteAsync(id, instructorId);
            return Ok(result);
        }

        // Get routines linked to a workout
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByWorkoutId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetRoutinesByWorkoutIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises linked to a workout
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByWorkoutId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetExercisesByWorkoutIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get all workouts linked to a user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetWorkoutsByUserId(int userId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByUserIdAsync(userId, pagination);
            return Ok(result);
        }

        // Get workouts by level
        [HttpGet("level/{levelId}")]
        public async Task<IActionResult> GetWorkoutsByLevel(int levelId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByLevelIdAsync(levelId, instructorId, pagination);
            return Ok(result);
        }

        #region Goal

        // Get workouts by goal
        [HttpGet("goal/{goalId}")]
        public async Task<IActionResult> GetWorkoutsByGoal(int goalId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByGoalIdAsync(goalId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/goals/{goalId}")]
        public async Task<IActionResult> AddGoal(int workoutId, int goalId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddGoalToWorkoutAsync(workoutId, goalId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/goals/{goalId}")]
        public async Task<IActionResult> RemoveGoal(int workoutId, int goalId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveGoalFromWorkoutAsync(workoutId, goalId, instructorId);
            return Ok(result);
        }

        #endregion

        #region Type

        // Get workouts by type
        [HttpGet("type/{typeId}")]
        public async Task<IActionResult> GetWorkoutsByType(int typeId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByTypeIdAsync(typeId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/types/{typeId}")]
        public async Task<IActionResult> AddType(int workoutId, int typeId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddTypeToWorkoutAsync(workoutId, typeId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/types/{typeId}")]
        public async Task<IActionResult> RemoveType(int workoutId, int typeId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveTypeFromWorkoutAsync(workoutId, typeId, instructorId);
            return Ok(result);
        }

        #endregion

        #region Modality

        // Get workouts by modality
        [HttpGet("modality/{modalityId}")]
        public async Task<IActionResult> GetWorkoutsByModality(int modalityId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByModalityIdAsync(modalityId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/modalities/{modalityId}")]
        public async Task<IActionResult> AddModality(int workoutId, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddModalityToWorkoutAsync(workoutId, modalityId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/modalities/{modalityId}")]
        public async Task<IActionResult> RemoveModality(int workoutId, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveModalityFromWorkoutAsync(workoutId, modalityId, instructorId);
            return Ok(result);
        }

        #endregion

        #region Hashtag

        // Get workouts by hashtag
        [HttpGet("hashtag/{hashtagId}")]
        public async Task<IActionResult> GetWorkoutsByHashtag(int hashtagId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByHashtagIdAsync(hashtagId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/hashtags/{hashtagId}")]
        public async Task<IActionResult> AddHashtag(int workoutId, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddHashtagToWorkoutAsync(workoutId, hashtagId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/hashtags/{hashtagId}")]
        public async Task<IActionResult> RemoveHashtag(int workoutId, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveHashtagFromWorkoutAsync(workoutId, hashtagId, instructorId);
            return Ok(result);
        }

        #endregion

        #region Routine

        // Get workouts by routine
        [HttpGet("routine/{routineId}")]
        public async Task<IActionResult> GetWorkoutsByRoutine(int routineId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByRoutineIdAsync(routineId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/routines/{routineId}")]
        public async Task<IActionResult> AddRoutine(int workoutId, int routineId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddRoutineToWorkoutAsync(workoutId, routineId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/routines/{routineId}")]
        public async Task<IActionResult> RemoveRoutine(int workoutId, int routineId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveRoutineFromWorkoutAsync(workoutId, routineId, instructorId);
            return Ok(result);
        }

        #endregion

        #region Exercise

        // Get workouts by exercise
        [HttpGet("exercise/{exerciseId}")]
        public async Task<IActionResult> GetWorkoutsByExercise(int exerciseId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _workoutService.GetByExerciseIdAsync(exerciseId, instructorId, pagination);
            return Ok(result);
        }

        [HttpPost("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> AddExercise(int workoutId, int exerciseId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.AddExerciseToWorkoutAsync(workoutId, exerciseId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise(int workoutId, int exerciseId, [FromQuery] int instructorId)
        {
            var result = await _workoutService.RemoveExerciseFromWorkoutAsync(workoutId, exerciseId, instructorId);
            return Ok(result);
        }

        #endregion

    }
}
