﻿using System;
using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Domain.Infrastructure.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
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
        IVideoRepository Videos { get; }

        // Transaction management
        //Task BeginTransactionAsync();
        //Task CommitAsync();
        //Task RollbackAsync();

        // Persistence
        Task SaveAsync();
        //Task SaveAndCommitAsync();

        // Utility
        //Task BeginAndCommitAsync(Func<Task> operation);
        bool HasPendingChanges();
        new void Dispose();
    }
}