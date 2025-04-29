using Application.DTOs;
using Application.DTOs.Goal;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class GoalController(IGoalService goalService) : ControllerBase
    {
        private readonly IGoalService _goalService = goalService;

        // Get a goal by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _goalService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get all goals with pagination
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _goalService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new goal
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGoalInputDTO dto)
        {
            var result = await _goalService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing goal
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGoalInputDTO dto)
        {
            dto.Id = id;
            var result = await _goalService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete a goal by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _goalService.DeleteAsync(id);
            return Ok(result);
        }

        // Get workouts linked to a goal (by instructor)
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByGoalId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _goalService.GetWorkoutsByGoalIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get routines linked to a goal (by instructor)
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByGoalId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _goalService.GetRoutinesByGoalIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises linked to a goal (by instructor)
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByGoalId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _goalService.GetExercisesByGoalIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
