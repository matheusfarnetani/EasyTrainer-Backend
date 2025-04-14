using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IGenericService<TCreateDTO, TUpdateDTO, TOutputDTO>
    {
        Task<TOutputDTO> CreateAsync(TCreateDTO dto);
        Task<TOutputDTO> UpdateAsync(TUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<TOutputDTO> GetByIdAsync(int id);
        Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(PaginationRequestDTO pagination);
    }
}
