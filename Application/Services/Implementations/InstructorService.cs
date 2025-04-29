using Application.DTOs;
using Application.DTOs.Exercise;
using Application.DTOs.Instructor;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Main;
using Domain.Infrastructure.Persistence;
using FluentValidation;

namespace Application.Services.Implementations
{
    public class InstructorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateInstructorInputDTO> createValidator,
        IValidator<UpdateInstructorInputDTO> updateValidator,
        IValidator<IdInputDTO> idValidator,
        IValidator<EmailInputDTO> emailValidator) : GenericService<Instructor, CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>(unitOfWork, mapper), IInstructorService
    {
        private new readonly IUnitOfWork _unitOfWork = unitOfWork;
        private new readonly IMapper _mapper = mapper;

        private readonly IValidator<CreateInstructorInputDTO> _createValidator = createValidator;
        private readonly IValidator<UpdateInstructorInputDTO> _updateValidator = updateValidator;
        private readonly IValidator<IdInputDTO> _idValidator = idValidator;
        private readonly IValidator<EmailInputDTO> _emailValidator = emailValidator;

        // General
        public override async Task<ServiceResponseDTO<InstructorOutputDTO>> CreateAsync(CreateInstructorInputDTO dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<Instructor>(dto);
            await _unitOfWork.Instructors.AddAsync(entity);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(entity));
        }

        public override async Task<ServiceResponseDTO<InstructorOutputDTO>> UpdateAsync(UpdateInstructorInputDTO dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var instructor = await _unitOfWork.Instructors.GetByIdAsync(dto.Id);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found.");

            if (dto.Name != null) instructor.Name = dto.Name;
            if (dto.Email != null) instructor.Email = dto.Email;
            if (dto.Password != null) instructor.Password = dto.Password;
            if (dto.MobileNumber != null) instructor.MobileNumber = dto.MobileNumber;
            if (dto.Birthday.HasValue) instructor.Birthday = dto.Birthday.Value;
            if (dto.Gender.HasValue) instructor.Gender = dto.Gender.Value;

            await _unitOfWork.Instructors.UpdateAsync(instructor);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        public override async Task<ServiceResponseDTO<bool>> DeleteAsync(int id)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = id });

            var entity = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (entity == null)
                return ServiceResponseDTO<bool>.CreateFailure("Instructor not found.");

            await _unitOfWork.Instructors.DeleteByIdAsync(id);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetByEmailAsync(string email)
        {
            await _emailValidator.ValidateAndThrowAsync(new EmailInputDTO { Email = email });

            var instructor = await _unitOfWork.Instructors.GetInstructorByEmailAsync(email);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        // User
        public async Task<ServiceResponseDTO<bool>> AddUserToInstructorAsync(int instructorId, int userId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            await _unitOfWork.Users.AddInstructorToUserAsync(userId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<bool>> RemoveUserFromInstructorAsync(int instructorId, int userId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            await _unitOfWork.Users.RemoveInstructorFromUserAsync(userId, instructorId);
            await _unitOfWork.SaveAndCommitAsync();

            return ServiceResponseDTO<bool>.CreateSuccess(true);
        }

        public async Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = userId });

            var instructors = await _unitOfWork.Instructors.GetInstructorsByUserIdAsync(userId);
            var result = PaginationHelper.Paginate<Instructor, InstructorOutputDTO>(instructors, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>.CreateSuccess(result);
        }

        // Workout
        public async Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var workouts = await _unitOfWork.Workouts.GetWorkoutsByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<Workout, WorkoutOutputDTO>(workouts, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetByWorkoutIdAsync(int workoutId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = workoutId });

            var instructor = await _unitOfWork.Instructors.GetInstructorByWorkoutIdAsync(workoutId);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found for this workout.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        // Routine
        public async Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var routines = await _unitOfWork.Routines.GetRoutinesByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<Routine, RoutineOutputDTO>(routines, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetByRoutineIdAsync(int routineId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = routineId });

            var instructor = await _unitOfWork.Instructors.GetInstructorByRoutineIdAsync(routineId);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found for this routine.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }

        // Exercise
        public async Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesAsync(int instructorId, PaginationRequestDTO pagination)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = instructorId });

            var exercises = await _unitOfWork.Exercises.GetExercisesByInstructorIdAsync(instructorId);
            var result = PaginationHelper.Paginate<Exercise, ExerciseOutputDTO>(exercises, pagination, _mapper);

            return ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>.CreateSuccess(result);
        }

        public async Task<ServiceResponseDTO<InstructorOutputDTO>> GetByExerciseIdAsync(int exerciseId)
        {
            await _idValidator.ValidateAndThrowAsync(new IdInputDTO { Id = exerciseId });

            var instructor = await _unitOfWork.Instructors.GetInstructorByExerciseIdAsync(exerciseId);
            if (instructor == null)
                return ServiceResponseDTO<InstructorOutputDTO>.CreateFailure("Instructor not found for this exercise.");

            return ServiceResponseDTO<InstructorOutputDTO>.CreateSuccess(_mapper.Map<InstructorOutputDTO>(instructor));
        }
    }
}
