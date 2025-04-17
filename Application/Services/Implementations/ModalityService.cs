using Application.DTOs;
using Application.DTOs.Modality;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure;

namespace Application.Services.Implementations
{
    public class ModalityService : GenericService<Modality, CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>, IModalityService
    {
        private readonly IModalityRepository _modalityRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateModalityInputDTO> _createValidator;
        private readonly IValidator<UpdateModalityInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public ModalityService(
            IModalityRepository modalityRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateModalityInputDTO> createValidator,
            IValidator<UpdateModalityInputDTO> updateValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(modalityRepository, mapper)
        {
            _modalityRepository = modalityRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _modalityIdValidator = modalityIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        public override async Task<ModalityOutputDTO> CreateAsync(CreateModalityInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Modality>(dto);
            await _modalityRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ModalityOutputDTO>(entity);
        }

        public override async Task<ModalityOutputDTO> UpdateAsync(UpdateModalityInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var modality = await _modalityRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) modality.Name = dto.Name;

            await _modalityRepository.UpdateAsync(modality);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ModalityOutputDTO>(modality);
        }

        public override async Task DeleteAsync(int id)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _modalityRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId, instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination)
        {
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
