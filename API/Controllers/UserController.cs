using Application.DTOs;
using Application.DTOs.User;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _userService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var response = await _userService.GetAllAsync(pagination);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserInputDTO dto)
        {
            var response = await _userService.CreateAsync(dto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserInputDTO dto)
        {
            dto.Id = id;
            var response = await _userService.UpdateAsync(dto);
            return Ok(response);
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDTO dto)
        {
            var success = await _userService.UpdatePasswordAsync(id, dto.CurrentPassword, dto.NewPassword);
            if (!success)
                return BadRequest(new { success = false, message = "Incorrect current password or user not found." });

            return Ok(new { success = true, message = "Password updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteAsync(id);
            return Ok(response);
        }

        // --- Goals ---
        [HttpGet("{id}/goals")]
        public async Task<IActionResult> GetGoals(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var response = await _userService.GetGoalsByUserIdAsync(id, pagination);
            return Ok(response);
        }

        [HttpPost("{id}/goals/{goalId}")]
        public async Task<IActionResult> AddGoal(int id, int goalId)
        {
            await _userService.AddGoalToUserAsync(id, goalId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Goal added successfully."));
        }

        [HttpDelete("{id}/goals/{goalId}")]
        public async Task<IActionResult> RemoveGoal(int id, int goalId)
        {
            await _userService.RemoveGoalFromUserAsync(id, goalId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Goal removed successfully."));
        }

        // --- Instructors ---
        [HttpGet("{id}/instructors")]
        public async Task<IActionResult> GetInstructors(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var response = await _userService.GetInstructorsByUserIdAsync(id, pagination);
            return Ok(response);
        }

        [HttpPost("{id}/instructors/{instructorId}")]
        public async Task<IActionResult> AddInstructor(int id, int instructorId)
        {
            await _userService.AddInstructorToUserAsync(id, instructorId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Instructor added successfully."));
        }

        [HttpDelete("{id}/instructors/{instructorId}")]
        public async Task<IActionResult> RemoveInstructor(int id, int instructorId)
        {
            await _userService.RemoveInstructorFromUserAsync(id, instructorId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Instructor removed successfully."));
        }

        // --- Workouts ---
        [HttpGet("{id}/workouts")]
        public async Task<IActionResult> GetWorkouts(int id, [FromQuery] PaginationRequestDTO pagination)
        {
            var response = await _userService.GetWorkoutsByUserIdAsync(id, pagination);
            return Ok(response);
        }

        [HttpPost("{id}/workouts/{workoutId}")]
        public async Task<IActionResult> AddWorkout(int id, int workoutId, [FromQuery] int instructorId)
        {
            await _userService.AddWorkoutToUserAsync(id, workoutId, instructorId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Workout added successfully."));
        }

        [HttpDelete("{id}/workouts/{workoutId}")]
        public async Task<IActionResult> RemoveWorkout(int id, int workoutId, [FromQuery] int instructorId)
        {
            await _userService.RemoveWorkoutFromUserAsync(id, workoutId, instructorId);
            return Ok(ServiceResponseDTO<string>.CreateSuccess("Workout removed successfully."));
        }
    }
}
