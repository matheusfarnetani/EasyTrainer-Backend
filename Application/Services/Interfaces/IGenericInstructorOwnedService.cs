using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IGenericInstructorOwnedService<TCreateDTO, TUpdateDTO, TOutputDTO>
    {
        Task<TOutputDTO> CreateAsync(TCreateDTO dto, int instructorId);
        Task<TOutputDTO> UpdateAsync(TUpdateDTO dto, int instructorId);
        Task DeleteAsync(int id, int instructorId);
        Task<TOutputDTO> GetByIdAsync(int id, int instructorId);
        Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(int instructorId, PaginationRequestDTO pagination);
    }
}
