using Application.DTOs;
using Application.DTOs.User;
using Application.DTOs.Workout;
using Application.Exceptions;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities.Main;
using Domain.RepositoryInterfaces;
using Domain.SystemInterfaces;
using AutoMapper;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class UserService : GenericService<User, CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateUserInputDTO> _createValidator;
        private readonly IValidator<UpdateUserInputDTO> _updateValidator;

        private readonly IValidator<IdInputDTO> _userIdValidator;
        private readonly IValidator<EmailInputDTO> _userEmailValidator;
        private readonly IValidator<IdInputDTO> _instructorIdValidator;
        private readonly IValidator<IdInputDTO> _goalIdValidator;
        private readonly IValidator<IdInputDTO> _levelIdValidator;
        private readonly IValidator<IdInputDTO> _workoutIdValidator;

        public UserService(
            IUserRepository userRepository,
            IWorkoutRepository workoutRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateUserInputDTO> createValidator,
            IValidator<UpdateUserInputDTO> updateValidator,
            IValidator<IdInputDTO> userIdValidator,
            IValidator<EmailInputDTO> emailValidator,
            IValidator<IdInputDTO> instructorIdValidator,
            IValidator<IdInputDTO> goalIdValidator,
            IValidator<IdInputDTO> levelIdValidator,
            IValidator<IdInputDTO> workoutIdValidator)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _workoutRepository = workoutRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _createValidator = createValidator;
            _updateValidator = updateValidator;

            _userIdValidator = userIdValidator;
            _userEmailValidator = emailValidator;
            _instructorIdValidator = instructorIdValidator;
            _goalIdValidator = goalIdValidator;
            _levelIdValidator = levelIdValidator;
            _workoutIdValidator = workoutIdValidator;
        }

        public override async Task<UserOutputDTO> CreateAsync(CreateUserInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<User>(dto);
            await _userRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserOutputDTO>(entity);
        }

        public override async Task<UserOutputDTO> UpdateAsync(UpdateUserInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var user = await _userRepository.GetByIdAsync(dto.Id);

            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.MobileNumber != null) user.MobileNumber = dto.MobileNumber;
            if (dto.Birthday.HasValue) user.Birthday = dto.Birthday.Value;
            if (dto.Weight.HasValue) user.Weight = dto.Weight.Value;
            if (dto.Height.HasValue) user.Height = dto.Height.Value;
            if (dto.Gender.HasValue) user.Gender = dto.Gender.Value;
            if (dto.Password != null) user.Password = dto.Password;
            if (dto.LevelId.HasValue) user.LevelId = dto.LevelId.Value;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UserOutputDTO>(user);
        }

        public override async Task DeleteAsync(int id)
        {
            await _userIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            await _userRepository.DeleteByIdAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserOutputDTO> GetByEmailAsync(string email)
        {
            await _userEmailValidator.ValidateAndThrowAsync(new EmailInputDTO { Email = email });

            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserOutputDTO>(user);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _instructorIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var users = await _userRepository.GetUsersByInstructorIdAsync(instructorId);
            return PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination)
        {
            await _goalIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });

            var users = await _userRepository.GetUsersByGoalIdAsync(goalId);
            return PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination)
        {
            await _levelIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });

            var users = await _userRepository.GetUsersByLevelIdAsync(levelId);
            return PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<UserOutputDTO>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination)
        {
            await _workoutIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var users = await _userRepository.GetUsersByWorkoutIdAsync(workoutId);
            return PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);
        }

        public async Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsAsync(int userId, PaginationRequestDTO pagination)
        {
            await _userIdValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var workouts = await _workoutRepository.GetWorkoutsByUserIdAsync(userId);
            return PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);
        }
    }
}
