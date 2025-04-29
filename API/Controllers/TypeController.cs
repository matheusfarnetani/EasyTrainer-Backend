using Application.DTOs;
using Application.DTOs.TrainingType;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class TypeController(ITypeService typeService) : ControllerBase
    {
        private readonly ITypeService _typeService = typeService;

        // Get a type by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _typeService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get all types with pagination
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _typeService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new type
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTypeInputDTO dto)
        {
            var result = await _typeService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing type
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTypeInputDTO dto)
        {
            dto.Id = id;
            var result = await _typeService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete a type by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _typeService.DeleteAsync(id);
            return Ok(result);
        }

        // Get workouts linked to this type (filtered by instructor)
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByTypeId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _typeService.GetWorkoutsByTypeIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get routines linked to this type (filtered by instructor)
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByTypeId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _typeService.GetRoutinesByTypeIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises linked to this type (filtered by instructor)
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByTypeId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _typeService.GetExercisesByTypeIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
