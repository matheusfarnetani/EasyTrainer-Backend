using Domain.Infrastructure.Persistence;
using Domain.Infrastructure.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;

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
    IRoutineHasExerciseRepository routineHasExercises,
    IVideoRepository videos
) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = logger;

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
    public IVideoRepository Videos { get; } = videos;

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    public bool HasPendingChanges() => _context.ChangeTracker.HasChanges();

    public void Dispose()
    {
        _context.Dispose();
    }
}
