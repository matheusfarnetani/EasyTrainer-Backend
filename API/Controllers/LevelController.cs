using Application.DTOs;
using Application.DTOs.Level;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class LevelController(ILevelService levelService) : ControllerBase
    {
        private readonly ILevelService _levelService = levelService;

        // Get a level by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _levelService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get all levels with pagination
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _levelService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new level
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLevelInputDTO dto)
        {
            var result = await _levelService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing level
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLevelInputDTO dto)
        {
            dto.Id = id;
            var result = await _levelService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete a level by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _levelService.DeleteAsync(id);
            return Ok(result);
        }

        // Get workouts with this level (filtered by instructor)
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByLevel(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _levelService.GetWorkoutsByLevelIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get routines with this level (filtered by instructor)
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByLevel(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _levelService.GetRoutinesByLevelIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises with this level (filtered by instructor)
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByLevel(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _levelService.GetExercisesByLevelIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
