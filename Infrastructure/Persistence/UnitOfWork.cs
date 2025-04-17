﻿using System;
using System.Threading.Tasks;
using Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Domain.Infrastructure.RepositoriesInterfaces;
using Domain.Infrastructure;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(
            AppDbContext context,
            ILogger<UnitOfWork> logger,
            IUserRepository users,
            IInstructorRepository instructors,
            IGoalRepository goals,
            ILevelRepository levels,
            ITypeRepository types,
            IModalityRepository modalities,
            IHashtagRepository hashtags,
            IWorkoutRepository workouts,
            IRoutineRepository routines,
            IExerciseRepository exercises,
            IRoutineHasExerciseRepository routineHasExercises)
        {
            _context = context;
            _logger = logger;
            Users = users;
            Instructors = instructors;
            Goals = goals;
            Levels = levels;
            Types = types;
            Modalities = modalities;
            Hashtags = hashtags;
            Workouts = workouts;
            Routines = routines;
            Exercises = exercises;
            RoutineHasExercises = routineHasExercises;
        }

        public IUserRepository Users { get; }
        public IInstructorRepository Instructors { get; }
        public IGoalRepository Goals { get; }
        public ILevelRepository Levels { get; }
        public ITypeRepository Types { get; }
        public IModalityRepository Modalities { get; }
        public IHashtagRepository Hashtags { get; }
        public IWorkoutRepository Workouts { get; }
        public IRoutineRepository Routines { get; }
        public IExerciseRepository Exercises { get; }
        public IRoutineHasExerciseRepository RoutineHasExercises { get; }

        private IDbContextTransaction? _currentTransaction;

        public async Task BeginTransactionAsync(int userId)
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlRawAsync($"SET @user_id = {userId};");
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CommitAsync");
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task SaveAndCommitAsync()
        {
            await SaveAsync();
            await CommitAsync();
        }

        public async Task<TResult> BeginAndCommitAsync<TResult>(int userId, Func<Task<TResult>> operation)
        {
            await BeginTransactionAsync(userId);

            try
            {
                var result = await operation();
                await SaveAndCommitAsync();
                return result;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task BeginAndCommitAsync(int userId, Func<Task> operation)
        {
            await BeginTransactionAsync(userId);

            try
            {
                await operation();
                await SaveAndCommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public bool HasPendingChanges() => _context.ChangeTracker.HasChanges();

        public void Dispose()
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
        }
    }
}
