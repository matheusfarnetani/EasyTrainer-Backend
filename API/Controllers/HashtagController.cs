using Application.DTOs;
using Application.DTOs.Hashtag;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class HashtagController(IHashtagService hashtagService) : ControllerBase
    {
        private readonly IHashtagService _hashtagService = hashtagService;

        // Get a hashtag by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _hashtagService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get all hashtags with pagination
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _hashtagService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new hashtag
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHashtagInputDTO dto)
        {
            var result = await _hashtagService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing hashtag
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHashtagInputDTO dto)
        {
            dto.Id = id;
            var result = await _hashtagService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete a hashtag by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hashtagService.DeleteAsync(id);
            return Ok(result);
        }

        // Get workouts linked to this hashtag
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkoutsByHashtagId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _hashtagService.GetWorkoutsByHashtagIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get routines linked to this hashtag
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutinesByHashtagId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _hashtagService.GetRoutinesByHashtagIdAsync(id, instructorId, pagination);
            return Ok(result);
        }

        // Get exercises linked to this hashtag
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByHashtagId(int id, [FromQuery] int instructorId, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _hashtagService.GetExercisesByHashtagIdAsync(id, instructorId, pagination);
            return Ok(result);
        }
    }
}
