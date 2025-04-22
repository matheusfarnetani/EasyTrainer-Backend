using Application.DTOs;
using Application.DTOs.Goal;
using Application.DTOs.Instructor;
using Application.DTOs.User;
using Application.DTOs.Workout;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class UserService : GenericService<User, CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>, IUserService
    {
        private readonly IValidator<CreateUserInputDTO> _createValidator;
        private readonly IValidator<UpdateUserInputDTO> _updateValidator;
        private readonly IValidator<IdInputDTO> _idValidator;
        private readonly IValidator<EmailInputDTO> _emailValidator;
        private new readonly IMapper _mapper;
        private new readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateUserInputDTO> createValidator,
            IValidator<UpdateUserInputDTO> updateValidator,
            IValidator<IdInputDTO> idValidator,
            IValidator<EmailInputDTO> emailValidator)
            : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _idValidator = idValidator;
            _emailValidator = emailValidator;
        }

        public override async Task<ServiceResponseDTO<UserOutputDTO>> CreateAsync(CreateUserInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<User>(dto);
            await _unitOfWork.Users.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<UserOutputDTO>.CreateSuccess(_mapper.Map<UserOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<UserOutputDTO>> UpdateAsync(UpdateUserInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var user = await _unitOfWork.Users.GetByIdAsync(dto.Id);
            if (user == null)
                return ServiceResponseDTO<UserOutputDTO>.CreateFailure("User not found.");

            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.MobileNumber != null) user.MobileNumber = dto.MobileNumber;
            if (dto.Birthday.HasValue) user.Birthday = dto.Birthday.Value;
            if (dto.Weight.HasValue) user.Weight = dto.Weight.Value;
            if (dto.Height.HasValue) user.Height = dto.Height.Value;
            if (dto.Gender.HasValue) user.Gender = dto.Gender.Value;
            if (dto.Password != null) user.Password = dto.Password;
            if (dto.LevelId.HasValue) user.LevelId = dto.LevelId.Value;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<UserOutputDTO>.CreateSuccess(_mapper.Map<UserOutputDTO>(user));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return ServiceResponseDTO<bool>.CreateFailure("User not found.");

            await _unitOfWork.Users.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<UserOutputDTO>> GetByEmailAsync(string email)
        {
            await _emailValidator.ValidateAndThrowAsync(new EmailInputDTO { Email = email });

            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null)
                return ServiceResponseDTO<UserOutputDTO>.CreateFailure("User not found.");

            return ServiceResponseDTO<UserOutputDTO>.CreateSuccess(_mapper.Map<UserOutputDTO>(user));
        }

        // Instructor
        public async Task<ServiceResponseDTO<bool>> AddInstructorToUserAsync(int userId, int instructorId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _unitOfWork.Users.AddInstructorToUserAsync(userId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveInstructorFromUserAsync(int userId, int instructorId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            await _unitOfWork.Users.RemoveInstructorFromUserAsync(userId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetInstructorsByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var instructors = await _unitOfWork.Users.GetInstructorsByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Instructor, InstructorOutputDTO>(instructors, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var users = await _unitOfWork.Users.GetUsersByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>.CreateSuccess(result);
        }

        // Level
        public async Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = levelId });

            var users = await _unitOfWork.Users.GetUsersByLevelIdAsync(levelId);
            var result = PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>.CreateSuccess(result);
        }

        // Goal
        public async Task<ServiceResponseDTO<bool>> AddGoalToUserAsync(int userId, int goalId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });

            await _unitOfWork.Users.AddGoalToUserAsync(userId, goalId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveGoalFromUserAsync(int userId, int goalId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });

            await _unitOfWork.Users.RemoveGoalFromUserAsync(userId, goalId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<GoalOutputDTO>>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var goals = await _unitOfWork.Users.GetGoalsByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Goal, GoalOutputDTO>(goals, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<GoalOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = goalId });

            var users = await _unitOfWork.Users.GetUsersByGoalIdAsync(goalId);
            var result = PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>.CreateSuccess(result);
        }

        // Workout
        public async Task<ServiceResponseDTO<bool>> AddWorkoutToUserAsync(int userId, int workoutId, int instructorId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            // 1. Workout belongs to Instructor?
            var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId);
            if (workout == null)
                return ServiceResponseDTO<bool>.CreateFailure("Workout not found.");

            if (workout.InstructorId != instructorId)
                return ServiceResponseDTO<bool>.CreateFailure("Workout does not belong to the instructor.");

            // 2. User is associated with Instructor?
            var instructorsOfUser = await _unitOfWork.Users.GetInstructorsByUserIdAsync(userId);
            var isLinked = instructorsOfUser.Any(i => i.Id == instructorId);

            if (!isLinked)
                return ServiceResponseDTO<bool>.CreateFailure("User is not associated with the instructor.");

            // 3. Create Relationship
            await _unitOfWork.Users.AddWorkoutToUserAsync(userId, workoutId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveWorkoutFromUserAsync(int userId, int workoutId, int instructorId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId);
            if (workout == null)
                return ServiceResponseDTO<bool>.CreateFailure("Workout not found.");

            if (workout.InstructorId != instructorId)
                return ServiceResponseDTO<bool>.CreateFailure("Workout does not belong to the instructor.");

            var instructorsOfUser = await _unitOfWork.Users.GetInstructorsByUserIdAsync(userId);
            var isLinked = instructorsOfUser.Any(i => i.Id == instructorId);

            if (!isLinked)
                return ServiceResponseDTO<bool>.CreateFailure("User is not associated with the instructor.");

            await _unitOfWork.Users.RemoveWorkoutFromUserAsync(userId, workoutId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var workouts = await _unitOfWork.Users.GetWorkoutsByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var users = await _unitOfWork.Users.GetUsersByWorkoutIdAsync(workoutId);
            var result = PaginationHelper.Paginate<User, UserOutputDTO>(users, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>.CreateSuccess(result);
        }
    }
}
