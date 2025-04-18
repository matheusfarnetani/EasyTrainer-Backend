using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using Application.Services.Interfaces;
using AutoMapper;
using Application.Exceptions;

namespace Application.Services.Implementations
{
    public abstract class GenericService<TEntity, TCreateDTO, TUpdateDTO, TOutputDTO> : IGenericService<TCreateDTO, TUpdateDTO, TOutputDTO>
        where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TOutputDTO> CreateAsync(TCreateDTO dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task<TOutputDTO> UpdateAsync(TUpdateDTO dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
            await _repository.SaveAsync();
        }

        public virtual async Task<TOutputDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);

            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(PaginationRequestDTO pagination)
        {
            var entities = await _repository.GetAllAsync();

            var paginated = entities
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<TOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = entities.Count(),
                Data = _mapper.Map<List<TOutputDTO>>(paginated)
            };
        }
    }
}
