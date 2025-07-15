using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Application.Services.Implementations
{
    public abstract class GenericInstructorOwnedService<TEntity, TCreateDTO, TUpdateDTO, TOutputDTO>
        : IGenericInstructorOwnedService<TCreateDTO, TUpdateDTO, TOutputDTO>
        where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericInstructorOwnedService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = ResolveRepository();
        }

        public virtual async Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto, int instructorId)
        {
            var entity = _mapper.Map<TEntity>(dto);
            SetInstructorId(entity, instructorId);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        public virtual async Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto, int instructorId)
        {
            var entity = _mapper.Map<TEntity>(dto);

            if (!CheckInstructorOwnership(entity, instructorId))
                return ServiceResponseDTO<TOutputDTO>.CreateFailure("You are not authorized to update this entity.");

            await _repository.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        public virtual async Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ServiceResponseDTO<bool>.CreateFailure("Entity not found.");

            if (!CheckInstructorOwnership(entity, instructorId))
                return ServiceResponseDTO<bool>.CreateFailure("You are not authorized to delete this entity.");

            await _repository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public virtual async Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id, int instructorId)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ServiceResponseDTO<TOutputDTO>.CreateFailure("Entity not found.");

            if (!CheckInstructorOwnership(entity, instructorId))
                return ServiceResponseDTO<TOutputDTO>.CreateFailure("You are not authorized to access this entity.");

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        public virtual async Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(int instructorId, PaginationRequestDTO pagination)
        {
            var entities = await GetEntitiesWithIncludes(instructorId);
            var result = PaginationHelper.Paginate<TEntity, TOutputDTO>(entities, pagination, _mapper);
            return ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>.CreateSuccess(result);
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntitiesWithIncludes(int instructorId)
        {
            var entities = await _repository.GetAllAsync();
            return entities.Where(e => CheckInstructorOwnership(e, instructorId));
        }


        /// <summary>
        /// Resolves the repository based on TEntity using UnitOfWork.
        /// </summary>
        protected virtual IGenericRepository<TEntity> ResolveRepository()
        {
            return typeof(TEntity).Name switch
            {
                nameof(User) => (IGenericRepository<TEntity>)_unitOfWork.Users,
                nameof(Instructor) => (IGenericRepository<TEntity>)_unitOfWork.Instructors,
                nameof(Goal) => (IGenericRepository<TEntity>)_unitOfWork.Goals,
                nameof(Level) => (IGenericRepository<TEntity>)_unitOfWork.Levels,
                nameof(Type) => (IGenericRepository<TEntity>)_unitOfWork.Types,
                nameof(Modality) => (IGenericRepository<TEntity>)_unitOfWork.Modalities,
                nameof(Hashtag) => (IGenericRepository<TEntity>)_unitOfWork.Hashtags,
                nameof(Workout) => (IGenericRepository<TEntity>)_unitOfWork.Workouts,
                nameof(Routine) => (IGenericRepository<TEntity>)_unitOfWork.Routines,
                nameof(Exercise) => (IGenericRepository<TEntity>)_unitOfWork.Exercises,
                _ => throw new InvalidOperationException($"No repository configured for {typeof(TEntity).Name}")
            };
        }

        /// <summary>
        /// Assigns the InstructorId to the entity.
        /// </summary>
        protected abstract void SetInstructorId(TEntity entity, int instructorId);

        /// <summary>
        /// Validates if the given entity belongs to the instructor.
        /// </summary>
        protected abstract bool CheckInstructorOwnership(TEntity entity, int instructorId);
    }
}
