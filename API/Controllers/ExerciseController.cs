using Application.DTOs;
using Application.DTOs.Exercise;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class ExerciseController(IExerciseService exerciseService) : ControllerBase
    {
        private readonly IExerciseService _exerciseService = exerciseService;

        // CRUD
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] int instructorId, [FromBody] CreateExerciseInputDTO dto)
        {
            var result = await _exerciseService.CreateAsync(dto, instructorId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int instructorId, [FromBody] UpdateExerciseInputDTO dto)
        {
            var result = await _exerciseService.UpdateAsync(dto, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.DeleteAsync(id, instructorId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetAllAsync(instructorId, pagination);
            return Ok(result);
        }

        // Instructor
        [HttpGet("{id}/instructor")]
        public async Task<IActionResult> GetInstructorByExerciseId(int id)
        {
            var result = await _exerciseService.GetInstructorByExerciseIdAsync(id);
            return Ok(result);
        }

        // Goal
        [HttpPost("{id}/goals/{goalId}")]
        public async Task<IActionResult> AddGoal(int id, int goalId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.AddGoalToExerciseAsync(id, goalId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}/goals/{goalId}")]
        public async Task<IActionResult> RemoveGoal(int id, int goalId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.RemoveGoalFromExerciseAsync(id, goalId, instructorId);
            return Ok(result);
        }

        [HttpGet("goal/{goalId}")]
        public async Task<IActionResult> GetByGoalId(int goalId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByGoalIdAsync(goalId, instructorId, pagination);
            return Ok(result);
        }

        // Type
        [HttpPost("{id}/types/{typeId}")]
        public async Task<IActionResult> AddType(int id, int typeId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.AddTypeToExerciseAsync(id, typeId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}/types/{typeId}")]
        public async Task<IActionResult> RemoveType(int id, int typeId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.RemoveTypeFromExerciseAsync(id, typeId, instructorId);
            return Ok(result);
        }

        [HttpGet("type/{typeId}")]
        public async Task<IActionResult> GetByTypeId(int typeId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByTypeIdAsync(typeId, instructorId, pagination);
            return Ok(result);
        }

        // Modality
        [HttpPost("{id}/modalities/{modalityId}")]
        public async Task<IActionResult> AddModality(int id, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.AddModalityToExerciseAsync(id, modalityId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}/modalities/{modalityId}")]
        public async Task<IActionResult> RemoveModality(int id, int modalityId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.RemoveModalityFromExerciseAsync(id, modalityId, instructorId);
            return Ok(result);
        }

        [HttpGet("modality/{modalityId}")]
        public async Task<IActionResult> GetByModalityId(int modalityId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByModalityIdAsync(modalityId, instructorId, pagination);
            return Ok(result);
        }

        // Hashtag
        [HttpPost("{id}/hashtags/{hashtagId}")]
        public async Task<IActionResult> AddHashtag(int id, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.AddHashtagToExerciseAsync(id, hashtagId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}/hashtags/{hashtagId}")]
        public async Task<IActionResult> RemoveHashtag(int id, int hashtagId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.RemoveHashtagFromExerciseAsync(id, hashtagId, instructorId);
            return Ok(result);
        }

        [HttpGet("hashtag/{hashtagId}")]
        public async Task<IActionResult> GetByHashtagId(int hashtagId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByHashtagIdAsync(hashtagId, instructorId, pagination);
            return Ok(result);
        }

        // Level
        [HttpGet("level/{levelId}")]
        public async Task<IActionResult> GetByLevelId(int levelId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByLevelIdAsync(levelId, instructorId, pagination);
            return Ok(result);
        }

        // Routine
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByExerciseId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByRoutineIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        [HttpGet("routine/{routineId}")]
        public async Task<IActionResult> GetByRoutineId(int routineId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByRoutineIdAsync(routineId, instructorId, pagination);
            return Ok(result);
        }

        // Workout
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByExerciseId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByWorkoutIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        [HttpGet("workout/{workoutId}")]
        public async Task<IActionResult> GetByWorkoutId(int workoutId, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetByWorkoutIdAsync(workoutId, instructorId, pagination);
            return Ok(result);
        }

        // Variation
        [HttpPost("{id}/variations/{variationId}")]
        public async Task<IActionResult> AddVariation(int id, int variationId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.AddVariationToExerciseAsync(id, variationId, instructorId);
            return Ok(result);
        }

        [HttpDelete("{id}/variations/{variationId}")]
        public async Task<IActionResult> RemoveVariation(int id, int variationId, [FromQuery] int instructorId)
        {
            var result = await _exerciseService.RemoveVariationFromExerciseAsync(id, variationId, instructorId);
            return Ok(result);
        }

        [HttpGet("{id}/variations")]
        public async Task<IActionResult> GetVariationsByExerciseId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _exerciseService.GetVariationsByExerciseIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
