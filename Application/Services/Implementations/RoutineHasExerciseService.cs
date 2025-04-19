using Application.DTOs;
using Application.DTOs.RoutineHasExercise;
using Application.Exceptions;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Relations;
using Domain.Infrastructure.Persistence;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class RoutineHasExerciseService : IRoutineHasExerciseService
    {
        private readonly IRoutineHasExerciseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;

        public RoutineHasExerciseService(
            IRoutineHasExerciseRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _exerciseIdValidator = exerciseIdValidator;
        }

        public async Task<IEnumerable<RoutineHasExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            var items = await _repository.GetExercisesByRoutineIdAsync(routineId);
            return _mapper.Map<IEnumerable<RoutineHasExerciseOutputDTO>>(items);
        }

        public async Task<IEnumerable<RoutineHasExerciseOutputDTO>> GetRoutinesByExerciseIdAsync(int exerciseId)
        { 
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            var items = await _repository.GetRoutinesByExerciseIdAsync(exerciseId);
            return _mapper.Map<IEnumerable<RoutineHasExerciseOutputDTO>>(items);
        }

        public async Task<RoutineHasExerciseOutputDTO> AddAsync(CreateRoutineHasExerciseDTO dto)
        {
            var entity = _mapper.Map<RoutineHasExercise>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<RoutineHasExerciseOutputDTO>(entity);
        }

        public async Task<RoutineHasExerciseOutputDTO> UpdateAsync(UpdateRoutineHasExerciseDTO dto)
        {
            var entity = await _repository.GetByIdAsync(dto.RoutineId, dto.ExerciseId)
                ?? throw new EntityNotFoundException(nameof(RoutineHasExercise), dto.RoutineId);

            _mapper.Map(dto, entity);

            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();


            return _mapper.Map<RoutineHasExerciseOutputDTO>(entity);
        }

        public async Task DeleteAsync(int routineId, int exerciseId)
        {
            await _repository.DeleteByIdAsync(routineId, exerciseId);
            await _repository.SaveAsync();
        }
    }
}
