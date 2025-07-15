using System.Security.Claims;
using API.Helpers;
using Application.DTOs;
using Application.DTOs.Video;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/[controller]")]
    public class VideoController(IVideoService videoService) : ControllerBase
    {
        private readonly IVideoService _videoService = videoService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _videoService.GetByIdAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDTO pagination)
        {
            var response = await _videoService.GetAllAsync(pagination);
            return Ok(response);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var response = await _videoService.GetPendingVideosAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateVideoInputDTO dto)
        {
            var response = await _videoService.CreateAsync(dto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVideoInputDTO dto)
        {
            dto.Id = id;
            var response = await _videoService.UpdateAsync(dto);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("external-update/{id}")]
        public async Task<IActionResult> ExternalUpdate(int id, [FromBody] UpdateVideoInputDTO dto)
        {
            Console.WriteLine("Header X-External-Request: " + Request.Headers["X-External-Request"]);
            Console.WriteLine($"[DEBUG] ExternalUpdate chamado para id={id}");
            dto.Id = id;

            //var verifier = new ECDSAVerifier("Keys/processor_public.pem");
            //var signatureValid = verifier.VerifySignature(dto, dto.Signature ?? string.Empty);
            //Console.WriteLine($"Assinatura válida? {signatureValid}");

            //if (string.IsNullOrWhiteSpace(dto.Signature) || !signatureValid)
            //    return Unauthorized("Invalid ECDSA signature.");

            var response = await _videoService.UpdateAsync(dto, systemUserId: -1);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _videoService.DeleteAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyVideos([FromQuery] PaginationRequestDTO pagination)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                           ?? User.FindFirst("sub")
                           ?? User.FindFirst("id")
                           ?? User.FindFirst("user_id");

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("User ID claim is invalid.");

            var response = await _videoService.GetByUserIdAsync(userId, pagination);
            return Ok(response);
        }
    }
}
