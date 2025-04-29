using Application.DTOs;
using Application.DTOs.RoutineHasExercise;
using Application.Exceptions;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Relations;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class RoutineHasExerciseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<IdInputDTO> routineIdValidator,
        IValidator<IdInputDTO> exerciseIdValidator) : IRoutineHasExerciseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IValidator<IdInputDTO> _routineIdValidator = routineIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator = exerciseIdValidator;

        public async Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> AddAsync(CreateRoutineHasExerciseDTO dto)
        {
            var entity = _mapper.Map<RoutineHasExercise>(dto);
            await _unitOfWork.RoutineHasExercises.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();
            return ServiceResponseDTO<RoutineHasExerciseOutputDTO>.CreateSuccess(_mapper.Map<RoutineHasExerciseOutputDTO>(entity));
        }

        public async Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> UpdateAsync(UpdateRoutineHasExerciseDTO dto)
        {
            var entity = await GetOrThrowAsync(dto.RoutineId, dto.ExerciseId);

            if (dto.Order.HasValue) entity.Order = dto.Order.Value;
            if (dto.Sets.HasValue) entity.Sets = dto.Sets.Value;
            if (dto.Reps.HasValue) entity.Reps = dto.Reps.Value;
            if (dto.RestTime.HasValue) entity.RestTime = dto.RestTime.Value;
            if (dto.Note != null) entity.Note = dto.Note;
            if (dto.Day.HasValue) entity.Day = dto.Day.Value;
            if (dto.Week.HasValue) entity.Week = dto.Week.Value;
            if (dto.IsOptional.HasValue) entity.IsOptional = dto.IsOptional.Value;

            await _unitOfWork.RoutineHasExercises.UpdateAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<RoutineHasExerciseOutputDTO>.CreateSuccess(_mapper.Map<RoutineHasExerciseOutputDTO>(entity));
        }

        public async Task<ServiceResponseDTO<RoutineHasExerciseOutputDTO>> GetByIdAsync(int routineId, int exerciseId)
        {
            await ValidateIds(routineId, exerciseId);
            var entity = await _unitOfWork.RoutineHasExercises.GetByIdAsync(routineId, exerciseId);

            if (entity == null)
                return ServiceResponseDTO<RoutineHasExerciseOutputDTO>.CreateFailure("Relation not found.");

            return ServiceResponseDTO<RoutineHasExerciseOutputDTO>.CreateSuccess(_mapper.Map<RoutineHasExerciseOutputDTO>(entity));
        }

        public async Task<ServiceResponseDTO<bool>> DeleteAsync(int routineId, int exerciseId)
        {
            await GetOrThrowAsync(routineId, exerciseId);

            await _unitOfWork.RoutineHasExercises.DeleteByIdAsync(routineId, exerciseId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>> GetExercisesByRoutineIdAsync(int routineId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });

            var items = await _unitOfWork.RoutineHasExercises.GetExercisesByRoutineIdAsync(routineId);
            var dtos = _mapper.Map<IEnumerable<RoutineHasExerciseOutputDTO>>(items);

            return ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>.CreateSuccess(dtos);
        }

        public async Task<ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>> GetRoutinesByExerciseIdAsync(int exerciseId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });

            var items = await _unitOfWork.RoutineHasExercises.GetRoutinesByExerciseIdAsync(exerciseId);
            var dtos = _mapper.Map<IEnumerable<RoutineHasExerciseOutputDTO>>(items);

            return ServiceResponseDTO<IEnumerable<RoutineHasExerciseOutputDTO>>.CreateSuccess(dtos);
        }

        private async Task<RoutineHasExercise> GetOrThrowAsync(int routineId, int exerciseId)
        {
            await ValidateIds(routineId, exerciseId);

            var entity = await _unitOfWork.RoutineHasExercises.GetByIdAsync(routineId, exerciseId);
            return entity ?? throw new EntityNotFoundException(nameof(RoutineHasExercise), $"{routineId},{exerciseId}");
        }

        private async Task ValidateIds(int routineId, int exerciseId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
        }
    }
}
