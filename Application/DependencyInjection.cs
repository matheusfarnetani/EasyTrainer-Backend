using Microsoft.Extensions.DependencyInjection;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Application.Profiles;
using Application.DTOs.Routine;
using Application.DTOs.Workout;
using Application.DTOs;
using Application.Validators.Routine;
using Application.Validators.Workout;
using FluentValidation;
using Application.DTOs.User;
using Application.Validators.User;
using Application.DTOs.Instructor;
using Application.Validators.Instructor;
using Application.DTOs.Goal;
using Application.Validators.Goal;
using Application.DTOs.Level;
using Application.Validators.Level;
using Application.DTOs.TrainingType;
using Application.Validators.Type;
using Application.DTOs.Modality;
using Application.Validators.Modality;
using Application.DTOs.Hashtag;
using Application.Validators.Hashtag;
using Application.DTOs.Exercise;
using Application.Validators.Exercise;
using Application.Validators;
using Application.DTOs.Auth;
using Application.Validators.Auth;
using Application.DTOs.Video;
using Application.Validators.Video;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Auth
            services.AddScoped<IAuthService, AuthService>();

            // Services
            services.AddScoped<IDeletionValidationService, DeletionValidationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInstructorService, InstructorService>();
            services.AddScoped<IGoalService, GoalService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped<IModalityService, ModalityService>();
            services.AddScoped<IHashtagService, HashtagService>();
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IRoutineService, RoutineService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IRoutineHasExerciseService, RoutineHasExerciseService>();

            // Video
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Validators
            services.AddScoped<IValidator<CreateUserInputDTO>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUserInputDTO>, UpdateUserValidator>();
            services.AddScoped<IValidator<CreateInstructorInputDTO>, CreateInstructorValidator>();
            services.AddScoped<IValidator<UpdateInstructorInputDTO>, UpdateInstructorValidator>();
            services.AddScoped<IValidator<CreateGoalInputDTO>, CreateGoalValidator>();
            services.AddScoped<IValidator<UpdateGoalInputDTO>, UpdateGoalValidator>();
            services.AddScoped<IValidator<CreateLevelInputDTO>, CreateLevelValidator>();
            services.AddScoped<IValidator<UpdateLevelInputDTO>, UpdateLevelValidator>();
            services.AddScoped<IValidator<CreateTypeInputDTO>, CreateTypeValidator>();
            services.AddScoped<IValidator<UpdateTypeInputDTO>, UpdateTypeValidator>();
            services.AddScoped<IValidator<CreateModalityInputDTO>, CreateModalityValidator>();
            services.AddScoped<IValidator<UpdateModalityInputDTO>, UpdateModalityValidator>();
            services.AddScoped<IValidator<CreateHashtagInputDTO>, CreateHashtagValidator>();
            services.AddScoped<IValidator<UpdateHashtagInputDTO>, UpdateHashtagValidator>();
            services.AddScoped<IValidator<CreateWorkoutInputDTO>, CreateWorkoutValidator>();
            services.AddScoped<IValidator<UpdateWorkoutInputDTO>, UpdateWorkoutValidator>();
            services.AddScoped<IValidator<CreateRoutineInputDTO>, CreateRoutineValidator>();
            services.AddScoped<IValidator<UpdateRoutineInputDTO>, UpdateRoutineValidator>();
            services.AddScoped<IValidator<CreateExerciseInputDTO>, CreateExerciseValidator>();
            services.AddScoped<IValidator<UpdateExerciseInputDTO>, UpdateExerciseValidator>();
            services.AddScoped<IValidator<IdInputDTO>, IdInputValidator>();
            services.AddScoped<IValidator<EmailInputDTO>, EmailInputValidator>();
            services.AddScoped<IValidator<CreateUserRegisterDTO>, CreateUserRegisterValidator>();
            services.AddScoped<IValidator<CreateInstructorRegisterDTO>, CreateInstructorRegisterValidator>();

            // Video
            services.AddScoped<IValidator<CreateVideoInputDTO>, CreateVideoValidator>();
            services.AddScoped<IValidator<UpdateVideoInputDTO>, UpdateVideoValidator>();

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            return services;
        }
    }
}
