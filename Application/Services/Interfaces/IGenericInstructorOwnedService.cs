using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IGenericInstructorOwnedService<TCreateDTO, TUpdateDTO, TOutputDTO>
    {
        Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto, int instructorId);
        Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto, int instructorId);
        Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId);
        Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id, int instructorId);
        Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(int instructorId, PaginationRequestDTO pagination);
    }
}