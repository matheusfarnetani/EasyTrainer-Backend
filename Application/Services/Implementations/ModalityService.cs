using Application.DTOs;
using Application.DTOs.Modality;
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
    public class ModalityService : GenericService<Modality, CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>, IModalityService
    {
        private new readonly IUnitOfWork _unitOfWork;
        private new readonly IMapper _mapper;

        private readonly IValidator<CreateModalityInputDTO> _createValidator;
        private readonly IValidator<UpdateModalityInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public ModalityService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateModalityInputDTO> createValidator,
            IValidator<UpdateModalityInputDTO> updateValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _modalityIdValidator = modalityIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        // General
        public override async Task<ServiceResponseDTO<ModalityOutputDTO>> CreateAsync(CreateModalityInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Modality>(dto);
            await _unitOfWork.Modalities.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<ModalityOutputDTO>.CreateSuccess(_mapper.Map<ModalityOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<ModalityOutputDTO>> UpdateAsync(UpdateModalityInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var modality = await _unitOfWork.Modalities.GetByIdAsync(dto.Id);
            if (modality == null)
                return ServiceResponseDTO<ModalityOutputDTO>.CreateFailure("Modality not found.");

            if (dto.Name != null) modality.Name = dto.Name;
            if (dto.Description != null) modality.Description = dto.Description;

            await _unitOfWork.Modalities.UpdateAsync(modality);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<ModalityOutputDTO>.CreateSuccess(_mapper.Map<ModalityOutputDTO>(modality));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var modality = await _unitOfWork.Modalities.GetByIdAsync(id);
            if (modality == null)
                return ServiceResponseDTO<bool>.CreateFailure("Modality not found.");

            await _unitOfWork.Modalities.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByModalityIdAsync(modalityId, instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }
    }
}
