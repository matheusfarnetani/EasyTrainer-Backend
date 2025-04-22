using Application.DTOs;
using Application.DTOs.TrainingType;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class TypeService : GenericService<TrainingType, CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>, ITypeService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateTypeInputDTO> _createValidator;
        private readonly IValidator<UpdateTypeInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public TypeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTypeInputDTO> createValidator,
            IValidator<UpdateTypeInputDTO> updateValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _typeIdValidator = typeIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        // General
        public override async Task<ServiceResponseDTO<TypeOutputDTO>> CreateAsync(CreateTypeInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<TrainingType>(dto);
            await _unitOfWork.Types.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<TypeOutputDTO>.CreateSuccess(_mapper.Map<TypeOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<TypeOutputDTO>> UpdateAsync(UpdateTypeInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var type = await _unitOfWork.Types.GetByIdAsync(dto.Id);
            if (type == null)
                return ServiceResponseDTO<TypeOutputDTO>.CreateFailure("Type not found.");

            if (dto.Name != null) type.Name = dto.Name;
            if (dto.Description != null) type.Description = dto.Description;

            await _unitOfWork.Types.UpdateAsync(type);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<TypeOutputDTO>.CreateSuccess(_mapper.Map<TypeOutputDTO>(type));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var type = await _unitOfWork.Types.GetByIdAsync(id);
            if (type == null)
                return ServiceResponseDTO<bool>.CreateFailure("Type not found.");

            await _unitOfWork.Types.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByTypeIdAsync(typeId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }
    }
}
