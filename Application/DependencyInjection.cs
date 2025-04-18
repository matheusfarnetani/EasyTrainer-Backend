using Microsoft.Extensions.DependencyInjection;
using Application.Services.Implementations;
using Application.Services.Interfaces;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDeletionValidationService, DeleteValidationService>();

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
            services.AddScoped<RoutineHasExerciseService, RoutineHasExerciseService>();

            return services;
        }
    }
}
