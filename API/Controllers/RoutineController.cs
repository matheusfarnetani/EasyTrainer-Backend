using Application.DTOs;
using Application.DTOs.Routine;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class RoutineController(IRoutineService routineService) : ControllerBase
    {
        private readonly IRoutineService _routineService = routineService;

        // Get a routine by ID (must belong to instructor)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int instructorId)
        {
            var result = await _routineService.GetByIdAsync(id, instructorId);
            return Ok(result);
        }

        // Get all routines for instructor (paginated)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetAllAsync(instructorId, pagination);
            return Ok(result);
        }

        // Create routine
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoutineInputDTO dto, [FromQuery] int instructorId)
        {
            var result = await _routineService.CreateAsync(dto, instructorId);
            return Ok(result);
        }

        // Update routine
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoutineInputDTO dto, [FromQuery] int instructorId)
        {
            dto.Id = id;
            var result = await _routineService.UpdateAsync(dto, instructorId);
            return Ok(result);
        }

        // Delete routine
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int instructorId)
        {
            var result = await _routineService.DeleteAsync(id, instructorId);
            return Ok(result);
        }

        // Get all exercises from this routine
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByRoutineId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetExercisesByRoutineIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get all workouts that use this routine
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByRoutineId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetWorkoutsByRoutineIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        #region Goal

        [HttpPost("{routineId}/goals/{goalId}")]
        public async Task<IActionResult> AddGoal(int routineId, int goalId, [FromQuery] int instructorId)
        {
            var result = await _routineService.AddGoalToRoutineAsync(routineId, goalId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{routineId}/goals/{goalId}")]
        public async Task<IActionResult> RemoveGoal(int routineId, int goalId, [FromQuery] int instructorId)
        {
            var result = await _routineService.RemoveGoalFromRoutineAsync(routineId, goalId, instructorId);
            return Ok(result);
        }

        [HttpGet("goal/{goalId}")]
        public async Task<IActionResult> GetByGoalId(int goalId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByGoalIdAsync(goalId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

        #region Type

        [HttpPost("{routineId}/types/{typeId}")]
        public async Task<IActionResult> AddType(int routineId, int typeId, [FromQuery] int instructorId)
        {
            var result = await _routineService.AddTypeToRoutineAsync(routineId, typeId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{routineId}/types/{typeId}")]
        public async Task<IActionResult> RemoveType(int routineId, int typeId, [FromQuery] int instructorId)
        {
            var result = await _routineService.RemoveTypeFromRoutineAsync(routineId, typeId, instructorId);
            return Ok(result);
        }

        [HttpGet("type/{typeId}")]
        public async Task<IActionResult> GetByTypeId(int typeId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByTypeIdAsync(typeId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

        #region Modality

        [HttpPost("{routineId}/modalities/{modalityId}")]
        public async Task<IActionResult> AddModality(int routineId, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _routineService.AddModalityToRoutineAsync(routineId, modalityId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{routineId}/modalities/{modalityId}")]
        public async Task<IActionResult> RemoveModality(int routineId, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _routineService.RemoveModalityFromRoutineAsync(routineId, modalityId, instructorId);
            return Ok(result);
        }

        [HttpGet("modality/{modalityId}")]
        public async Task<IActionResult> GetByModalityId(int modalityId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByModalityIdAsync(modalityId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

        #region Hashtag

        [HttpPost("{routineId}/hashtags/{hashtagId}")]
        public async Task<IActionResult> AddHashtag(int routineId, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _routineService.AddHashtagToRoutineAsync(routineId, hashtagId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{routineId}/hashtags/{hashtagId}")]
        public async Task<IActionResult> RemoveHashtag(int routineId, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _routineService.RemoveHashtagFromRoutineAsync(routineId, hashtagId, instructorId);
            return Ok(result);
        }

        [HttpGet("hashtag/{hashtagId}")]
        public async Task<IActionResult> GetByHashtagId(int hashtagId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByHashtagIdAsync(hashtagId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

        #region Exercise

        [HttpPost("{routineId}/exercises/{exerciseId}")]
        public async Task<IActionResult> AddExercise(int routineId, int exerciseId, [FromQuery] int instructorId)
        {
            var result = await _routineService.AddExerciseToRoutineAsync(routineId, exerciseId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{routineId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise(int routineId, int exerciseId, [FromQuery] int instructorId)
        {
            var result = await _routineService.RemoveExerciseFromRoutineAsync(routineId, exerciseId, instructorId);
            return Ok(result);
        }

        [HttpGet("exercise/{exerciseId}")]
        public async Task<IActionResult> GetByExerciseId(int exerciseId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByExerciseIdAsync(exerciseId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

        #region Level

        [HttpGet("level/{levelId}")]
        public async Task<IActionResult> GetByLevelId(int levelId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _routineService.GetByLevelIdAsync(levelId, instructorId, pagination);
            return Ok(result);
        }

        #endregion

    }
}
