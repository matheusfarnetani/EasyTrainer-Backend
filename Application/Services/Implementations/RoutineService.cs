using Application.DTOs;
using Application.DTOs.Routine;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using AutoMapper;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure.Persistence;

namespace Application.Services.Implementations
{
    public class RoutineService : GenericInstructorOwnedService<Routine, CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>, IRoutineService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateRoutineInputDTO> _createValidator;
        private readonly IValidator<UpdateRoutineInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _typeIdValidator;
        private readonly IValidator<IdInputDTO> _modalityIdValidator;
        private readonly IValidator<IdInputDTO> _hashtagIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;

        public RoutineService(
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateRoutineInputDTO> createValidator,
            IValidator<UpdateRoutineInputDTO> updateValidator,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> typeIdValidator,
            IValidator<IdInputDTO> modalityIdValidator,
            IValidator<IdInputDTO> hashtagIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator)
            : base(routineRepository, mapper)
        {
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _routineIdValidator = routineIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _typeIdValidator = typeIdValidator;
            _modalityIdValidator = modalityIdValidator;
            _hashtagIdValidator = hashtagIdValidator;
            _exerciseIdValidator = exerciseIdValidator;
        }

        public override async Task<RoutineOutputDTO> CreateAsync(CreateRoutineInputDTO dto, int instructorId)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var entity = _mapper.Map<Routine>(dto);
            SetInstructorId(entity, instructorId);

            await _routineRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RoutineOutputDTO>(entity);
        }

        public override async Task<RoutineOutputDTO> UpdateAsync(UpdateRoutineInputDTO dto, int instructorId)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routine = await _routineRepository.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException(nameof(Routine), dto.Id);

            EnsureInstructorOwnership(routine, instructorId);

            if (dto.Name != null) routine.Name = dto.Name;
            if (dto.Description != null) routine.Description = dto.Description;

            await _routineRepository.UpdateAsync(routine);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RoutineOutputDTO>(routine);
        }

        public override async Task DeleteAsync(int id, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routine = await _routineRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Routine), id);

            EnsureInstructorOwnership(routine, instructorId);

            await _routineRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddGoalToRoutineAsync(int routineId, int goalId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.AddGoalToRoutineAsync(routineId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.RemoveGoalFromRoutineAsync(routineId, goalId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddTypeToRoutineAsync(int routineId, int typeId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.AddTypeToRoutineAsync(routineId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _typeIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = typeId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.RemoveTypeFromRoutineAsync(routineId, typeId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.AddModalityToRoutineAsync(routineId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _modalityIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = modalityId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.RemoveModalityFromRoutineAsync(routineId, modalityId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.AddHashtagToRoutineAsync(routineId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _hashtagIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = hashtagId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.RemoveHashtagFromRoutineAsync(routineId, hashtagId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.AddExerciseToRoutineAsync(routineId, exerciseId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _routineRepository.RemoveExerciseFromRoutineAsync(routineId, exerciseId, instructorId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<InstructorOutputDTO> GetInstructorByRoutineIdAsync(int routineId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });

            var instructor = await _routineRepository.GetInstructorByRoutineIdAsync(routineId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _exerciseRepository.GetExercisesByRoutineIdAsync(routineId, instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }

        protected override void SetInstructorId(Routine entity, int instructorId)
        {
            if (entity.InstructorId != 0 && entity.InstructorId != instructorId)
                throw new UnauthorizedAccessException("Cannot overwrite instructorId of an existing routine.");

            entity.InstructorId = instructorId;
        }

        protected override bool CheckInstructorOwnership(Routine entity, int instructorId)
        {
            return entity.InstructorId == instructorId;
        }

        private void EnsureInstructorOwnership(Routine entity, int instructorId)
        {
            if (!CheckInstructorOwnership(entity, instructorId))
                throw new UnauthorizedAccessException("This routine does not belong to the specified instructor.");
        }
    }
}
