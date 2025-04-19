using Application.DTOs;
using Application.DTOs.TrainingType;
using Application.DTOs.Workout;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.Helpers;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class TypeService : GenericService<TrainingType, CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>, ITypeService
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateTypeInputDTO> _createValidator;
        private readonly IValidator<UpdateTypeInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;

        public TypeService(
            ITypeRepository typeRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTypeInputDTO> createValidator,
            IValidator<UpdateTypeInputDTO> updateValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> instructorIdValidator)
            : base(typeRepository, mapper)
        {
            _typeRepository = typeRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _typeIdValidator = typeIdValidator;
            _instructorIdValidator = instructorIdValidator;
        }

        public override async Task<TypeOutputDTO> CreateAsync(CreateTypeInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<TrainingType>(dto);
            await _typeRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<TypeOutputDTO>(entity);
        }

        public override async Task<TypeOutputDTO> UpdateAsync(UpdateTypeInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var type = await _typeRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) type.Name = dto.Name;

            await _typeRepository.UpdateAsync(type);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<TypeOutputDTO>(type);
        }

        public override async Task DeleteAsync(int id)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _typeRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId, instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId, instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination)
        {
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
