using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.User;
using Application.DTOs.Workout;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using Application.Validators.Instructor;
using Application.Validators.User;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class InstructorService : GenericService<Instructor, CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>, IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IRoutineRepository _routineRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateInstructorInputDTO> _createValidator;
        private readonly IValidator<UpdateInstructorInputDTO> _updateValidator;

        private readonly IValidator<IdInputDTO> _userIdValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<EmailInputDTO> _instructorEmailValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator;
        private readonly IValidator<IdInputDTO> _routineIdValidator;
        private readonly IValidator<IdInputDTO> _exerciseIdValidator;

        public InstructorService(
            IInstructorRepository instructorRepository,
            IWorkoutRepository workoutRepository,
            IRoutineRepository routineRepository,
            IExerciseRepository exerciseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateInstructorInputDTO> createValidator,
            IValidator<UpdateInstructorInputDTO> updateValidator,
            IValidator<IdInputDTO> userIdValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<EmailInputDTO> instructorEmailValidator,
            IValidator<IdInputDTO> workoutIdValidator,
            IValidator<IdInputDTO> routineIdValidator,
            IValidator<IdInputDTO> exerciseIdValidator)
            : base(instructorRepository, mapper)
        {
            _instructorRepository = instructorRepository;
            _workoutRepository = workoutRepository;
            _routineRepository = routineRepository;
            _exerciseRepository = exerciseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _createValidator = createValidator;
            _updateValidator = updateValidator;

            _userIdValidator = userIdValidator;
            _instructorIdValidator = instructorIdValidator;
            _workoutIdValidator = workoutIdValidator;
            _routineIdValidator = routineIdValidator;
            _exerciseIdValidator = exerciseIdValidator;
        }

        public override async Task<InstructorOutputDTO> CreateAsync(CreateInstructorInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Instructor>(dto);
            await _instructorRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<InstructorOutputDTO>(entity);
        }

        public override async Task<InstructorOutputDTO> UpdateAsync(UpdateInstructorInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var instructor = await _instructorRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) instructor.Name = dto.Name;
            if (dto.Email != null) instructor.Email = dto.Email;
            if (dto.Password != null) instructor.Password = dto.Password;
            if (dto.MobileNumber != null) instructor.MobileNumber = dto.MobileNumber;
            if (dto.Birthday.HasValue) instructor.Birthday = dto.Birthday.Value;
            if (dto.Gender.HasValue) instructor.Gender = dto.Gender.Value;

            await _instructorRepository.UpdateAsync(instructor);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public override async Task DeleteAsync(int id)
        {
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _instructorRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<InstructorOutputDTO> GetByEmailAsync(string email)
        {
            await _instructorEmailValidator.ValidateAndThrowAsync(new EmailInputDTO { Email = email });

            var instructor = await _instructorRepository.GetInstructorByEmailAsync(email);
            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<InstructorOutputDTO>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _userIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var instructors = await _instructorRepository.GetInstructorsByUserIdAsync(userId);
            return PaginationHelper.Paginate<Instructor, InstructorOutputDTO>(instructors, pagination, _mapper);
        }

        public async Task<InstructorOutputDTO> GetByWorkoutIdAsync(int workoutId)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var instructor = await _instructorRepository.GetInstructorByWorkoutIdAsync(workoutId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<InstructorOutputDTO> GetByRoutineIdAsync(int routineId)
        {
            await _routineIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });

            var instructor = await _instructorRepository.GetInstructorByRoutineIdAsync(routineId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<InstructorOutputDTO> GetByExerciseIdAsync(int exerciseId)
        {
            await _exerciseIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });

            var instructor = await _instructorRepository.GetInstructorByExerciseIdAsync(exerciseId)
                ?? throw new EntityNotFoundException(nameof(Instructor), 0);

            return _mapper.Map<InstructorOutputDTO>(instructor);
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorRepository.ExistsByIdAsync(instructorId);

            var workouts = await _workoutRepository.GetWorkoutsByInstructorIdAsync(instructorId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorRepository.ExistsByIdAsync(instructorId);

            var routines = await _routineRepository.GetRoutinesByInstructorIdAsync(instructorId);
            return PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorRepository.ExistsByIdAsync(instructorId);

            var exercises = await _exerciseRepository.GetExercisesByInstructorIdAsync(instructorId);
            return PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);
        }
    }
}
