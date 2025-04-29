using Application.DTOs.RoutineHasExercise;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class RoutineHasExerciseController(IRoutineHasExerciseService service) : ControllerBase
    {
        private readonly IRoutineHasExerciseService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoutineHasExerciseDTO dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoutineHasExerciseDTO dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{routineId:int}/{exerciseId:int}")]
        public async Task<IActionResult> Delete(int routineId, int exerciseId)
        {
            var result = await _service.DeleteAsync(routineId, exerciseId);
            return Ok(result);
        }

        [HttpGet("{routineId:int}/{exerciseId:int}")]
        public async Task<IActionResult> GetById(int routineId, int exerciseId)
        {
            var result = await _service.GetByIdAsync(routineId, exerciseId);
            return Ok(result);
        }

        [HttpGet("routine/{routineId:int}")]
        public async Task<IActionResult> GetExercisesByRoutineId(int routineId)
        {
            var result = await _service.GetExercisesByRoutineIdAsync(routineId);
            return Ok(result);
        }

        [HttpGet("exercise/{exerciseId:int}")]
        public async Task<IActionResult> GetRoutinesByExerciseId(int exerciseId)
        {
            var result = await _service.GetRoutinesByExerciseIdAsync(exerciseId);
            return Ok(result);
        }
    }
}
