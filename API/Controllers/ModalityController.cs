using Application.DTOs;
using Application.DTOs.Modality;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class ModalityController(IModalityService modalityService) : ControllerBase
    {
        private readonly IModalityService _modalityService = modalityService;

        // Get a modality by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _modalityService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get all modalities with pagination
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _modalityService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new modality
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModalityInputDTO dto)
        {
            var result = await _modalityService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing modality
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateModalityInputDTO dto)
        {
            dto.Id = id;
            var result = await _modalityService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete a modality by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _modalityService.DeleteAsync(id);
            return Ok(result);
        }

        // Get workouts linked to this modality
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByModalityId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _modalityService.GetWorkoutsByModalityIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get routines linked to this modality
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByModalityId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _modalityService.GetRoutinesByModalityIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises linked to this modality
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByModalityId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _modalityService.GetExercisesByModalityIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
