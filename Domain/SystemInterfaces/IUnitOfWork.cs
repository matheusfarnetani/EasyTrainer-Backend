using System;
using Domain.RepositoryInterfaces;

namespace Domain.SystemInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IInstructorRepository Instructors { get; }
        IGoalRepository Goals { get; }
        ILevelRepository Levels { get; }
        ITypeRepository Types { get; }
        IModalityRepository Modalities { get; }
        IHashtagRepository Hashtags { get; }
        IWorkoutRepository Workouts { get; }
        IRoutineRepository Routines { get; }
        IExerciseRepository Exercises { get; }
        IRoutineHasExerciseRepository RoutineHasExercises { get; }

        Task BeginTransactionAsync(int userId);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
