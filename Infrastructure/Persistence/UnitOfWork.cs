using Domain.Infrastructure.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Infrastructure.Persistence;

namespace Infrastructure.Persistence
{
    public class UnitOfWork(
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
        IRoutineHasExerciseRepository routineHasExercises) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<UnitOfWork> _logger = logger;
        private IDbContextTransaction? _currentTransaction;

        public IUserRepository Users { get; } = users;
        public IInstructorRepository Instructors { get; } = instructors;
        public IGoalRepository Goals { get; } = goals;
        public ILevelRepository Levels { get; } = levels;
        public ITypeRepository Types { get; } = types;
        public IModalityRepository Modalities { get; } = modalities;
        public IHashtagRepository Hashtags { get; } = hashtags;
        public IWorkoutRepository Workouts { get; } = workouts;
        public IRoutineRepository Routines { get; } = routines;
        public IExerciseRepository Exercises { get; } = exercises;
        public IRoutineHasExerciseRepository RoutineHasExercises { get; } = routineHasExercises;

        public async Task BeginTransactionAsync(int userId)
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlAsync($"SET @user_id = {userId};");
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _context.SaveChangesAsync();
                    await _currentTransaction.CommitAsync();
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Commit failed. Rolling back.");
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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveAndCommitAsync()
        {
            await SaveAsync();
            await CommitAsync();
        }

        public async Task<TResult> BeginAndCommitAsync<TResult>(int userId, Func<Task<TResult>> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
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
            });
        }

        public async Task BeginAndCommitAsync(int userId, Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
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
            });
        }

        public bool HasPendingChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            _currentTransaction?.Dispose();
        }
    }
}
