using Application.DTOs;
using Application.Helpers;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Entities.Relations;
using Domain.Infrastructure.Persistence;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Application.Services.Implementations
{
    public class GenericService<TEntity, TCreateDTO, TUpdateDTO, TOutputDTO>
        where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = ResolveRepository();
        }

        /// <summary>
        /// Creates a new entity based on the input DTO and returns the result.
        /// </summary>
        public virtual async Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        /// <summary>
        /// Updates an existing entity based on the input DTO and returns the updated result.
        /// </summary>
        public virtual async Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        public virtual async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        public virtual async Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ServiceResponseDTO<TOutputDTO>.CreateFailure("Item not found.");

            return ServiceResponseDTO<TOutputDTO>.CreateSuccess(_mapper.Map<TOutputDTO>(entity));
        }

        /// <summary>
        /// Returns a paginated list of all entities.
        /// </summary>
        public virtual async Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(PaginationRequestDTO pagination)
        {
            var entities = await _repository.GetAllAsync();
            var paginated = PaginationHelper.Paginate<TEntity, TOutputDTO>(entities, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>.CreateSuccess(paginated);
        }

        /// <summary>
        /// Resolves the correct repository based on the generic entity type.
        /// </summary>
        protected IGenericRepository<TEntity> ResolveRepository()
        {
            return typeof(TEntity).Name switch
            {
                nameof(User) => (IGenericRepository<TEntity>)_unitOfWork.Users,
                nameof(Instructor) => (IGenericRepository<TEntity>)_unitOfWork.Instructors,
                nameof(Goal) => (IGenericRepository<TEntity>)_unitOfWork.Goals,
                nameof(Level) => (IGenericRepository<TEntity>)_unitOfWork.Levels,
                nameof(TrainingType) => (IGenericRepository<TEntity>)_unitOfWork.Types,
                nameof(Modality) => (IGenericRepository<TEntity>)_unitOfWork.Modalities,
                nameof(Hashtag) => (IGenericRepository<TEntity>)_unitOfWork.Hashtags,
                nameof(Workout) => (IGenericRepository<TEntity>)_unitOfWork.Workouts,
                nameof(Routine) => (IGenericRepository<TEntity>)_unitOfWork.Routines,
                nameof(Exercise) => (IGenericRepository<TEntity>)_unitOfWork.Exercises,
                nameof(RoutineHasExercise) => (IGenericRepository<TEntity>)_unitOfWork.RoutineHasExercises,
                _ => throw new InvalidOperationException($"No repository configured for {typeof(TEntity).Name}")
            };
        }
    }
}
