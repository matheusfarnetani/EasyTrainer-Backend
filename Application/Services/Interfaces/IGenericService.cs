using Application.DTOs;

namespace Application.Services.Interfaces
{
    public interface IGenericService<TCreateDTO, TUpdateDTO, TOutputDTO>
    {
        Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto);
        Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto);
        Task<ServiceResponseDTO<bool>> DeleteAsync(int id);
        Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id);
        Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(PaginationRequestDTO pagination);
    }
}