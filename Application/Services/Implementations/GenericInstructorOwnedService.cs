using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RepositoryInterfaces;
using Application.Exceptions;

namespace Application.Services.Implementations
{
    public abstract class GenericInstructorOwnedService<TEntity, TCreateDTO, TUpdateDTO, TOutputDTO> : IGenericInstructorOwnedService<TCreateDTO, TUpdateDTO, TOutputDTO>
        where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected GenericInstructorOwnedService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TOutputDTO> CreateAsync(TCreateDTO dto, int instructorId)
        {
            var entity = _mapper.Map<TEntity>(dto);
            SetInstructorId(entity, instructorId);

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task<TOutputDTO> UpdateAsync(TUpdateDTO dto, int instructorId)
        {
            var entity = _mapper.Map<TEntity>(dto);

            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedOperationException("You are not authorized to update this entity.");

            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();

            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task DeleteAsync(int id, int instructorId)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);

            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedOperationException("You are not authorized to delete this entity.");

            await _repository.DeleteByIdAsync(id);
            await _repository.SaveAsync();
        }

        public virtual async Task<TOutputDTO> GetByIdAsync(int id, int instructorId)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);

            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedOperationException("You are not authorized to access this entity.");

            return _mapper.Map<TOutputDTO>(entity);
        }

        public virtual async Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(int instructorId, PaginationRequestDTO pagination)
        {
            var entities = await _repository.GetAllAsync();

            // Filter entities that belong to the instructor
            var filtered = entities.Where(e => CheckInstructorOwnership(e, instructorId));

            var paginated = filtered
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<TOutputDTO>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = filtered.Count(),
                Data = _mapper.Map<List<TOutputDTO>>(paginated)
            };
        }

        /// <summary>
        /// Set the InstructorId on the entity.
        /// </summary>
        protected abstract void SetInstructorId(TEntity entity, int instructorId);

        /// <summary>
        /// Check if the entity belongs to the given InstructorId.
        /// </summary>
        protected abstract bool CheckInstructorOwnership(TEntity entity, int instructorId);
    }
}
