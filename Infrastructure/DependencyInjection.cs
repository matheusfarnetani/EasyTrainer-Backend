using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure.Persistence;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IModalityRepository, ModalityRepository>();
            services.AddScoped<IHashtagRepository, HashtagRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IRoutineRepository, RoutineRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IRoutineHasExerciseRepository, RoutineHasExerciseRepository>();

            // ConnectionManager and UnitOfWork
            services.AddScoped<IConnectionManager, ConnectionManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // DbContext with ConnectionManager
            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                var connectionManager = provider.GetRequiredService<IConnectionManager>();
                var connStr = connectionManager.GetCurrentConnectionString();

                options.UseMySql(
                    connStr,
                    new MySqlServerVersion(new Version(8, 0, 41)),
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null
                        );
                    });
            });


            // Health Check
            services.AddHealthChecks().AddDbContextCheck<AppDbContext>("Database");

            return services;
        }
    }
}
