using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();

            return services;
        }
    }
}
