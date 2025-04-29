using Application.DTOs;
using Application.DTOs.Instructor;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class InstructorController(IInstructorService instructorService) : ControllerBase
    {
        private readonly IInstructorService _instructorService = instructorService;

        // Get a specific instructor by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _instructorService.GetByIdAsync(id);
            return Ok(result);
        }

        // Get a paginated list of instructors
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _instructorService.GetAllAsync(pagination);
            return Ok(result);
        }

        // Create a new instructor
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInstructorInputDTO dto)
        {
            var result = await _instructorService.CreateAsync(dto);
            return Ok(result);
        }

        // Update an existing instructor
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorInputDTO dto)
        {
            dto.Id = id;
            var result = await _instructorService.UpdateAsync(dto);
            return Ok(result);
        }

        // Delete an instructor by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _instructorService.DeleteAsync(id);
            return Ok(result);
        }

        // Get all users assigned to a specific instructor
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _instructorService.GetByUserIdAsync(id, pagination);
            return Ok(result);
        }

        // Assign a user to an instructor
        [HttpPost("{instructorId}/users/{userId}")]
        public async Task<IActionResult> AddUserToInstructor(int instructorId, int userId)
        {
            var result = await _instructorService.AddUserToInstructorAsync(instructorId, userId);
            return Ok(result);
        }

        // Remove a user from an instructor
        [HttpDelete("{instructorId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromInstructor(int instructorId, int userId)
        {
            var result = await _instructorService.RemoveUserFromInstructorAsync(instructorId, userId);
            return Ok(result);
        }

        // Get all workouts created by a specific instructor
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkouts(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _instructorService.GetWorkoutsAsync(id, pagination);
            return Ok(result);
        }

        // Get all routines created by a specific instructor
        [HttpGet("{id}/routines")]
        public async Task<IActionResult> GetRoutines(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _instructorService.GetRoutinesAsync(id, pagination);
            return Ok(result);
        }

        // Get all exercises created by a specific instructor
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercises(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var result = await _instructorService.GetExercisesAsync(id, pagination);
            return Ok(result);
        }
    }
}
