# Application
## DTOs
- Data Transfer Objects (DTOs) are simple objects used to encapsulate data sent between the API and the application layer.
- Separate **Input DTOs** and **Output DTOs** for each Main Model.

### Inputs
- DTOs used for **Create** and **Update** operations.
- Contain only fields allowed to be written by the client.
- May include validation annotations (e.g., `[Required]`, `[MaxLength]`).
    - CreateUserInputDTO
    - UpdateUserInputDTO
    - CreateInstructorInputDTO
    - UpdateInstructorInputDTO
    - CreateGoalInputDTO
    - UpdateGoalInputDTO
    - CreateLevelInputDTO
    - UpdateLevelInputDTO
    - CreateTypeInputDTO
    - UpdateTypeInputDTO
    - CreateModalityInputDTO
    - UpdateModalityInputDTO
    - CreateHashtagInputDTO
    - UpdateHashtagInputDTO
    - CreateWorkoutInputDTO
    - UpdateWorkoutInputDTO
    - CreateRoutineInputDTO
    - UpdateRoutineInputDTO
    - CreateExerciseInputDTO
    - UpdateExerciseInputDTO
    - CreateUserHasGoalInputDTO
    - CreateWorkoutHasUserInputDTO
    - CreateRoutineHasExerciseInputDTO

---
### Outputs
- DTOs used for **Read** operations.
- May expose additional computed fields (e.g., `FullName`, `GoalNames`, etc.).
- Never expose sensitive fields (e.g., passwords).
    - UserOutputDTO
    - InstructorOutputDTO
    - GoalOutputDTO
    - LevelOutputDTO
    - TypeOutputDTO
    - ModalityOutputDTO
    - HashtagOutputDTO
    - WorkoutOutputDTO
    - RoutineOutputDTO
    - ExerciseOutputDTO
    - VariationOutputDTO
    - UserHasGoalOutputDTO
    - WorkoutHasUserOutputDTO
    - RoutineHasExerciseOutputDTO

---
### System's DTOs
#### Pagination
- DTOs: `PaginationRequestDTO`, `PaginationResponseDTO<T>`
- Purpose: Improve scalability by supporting paging in list operations.

```
public class PaginationRequestDTO
{
    public int PageNumber { get; set; } = 1;  // Default to first page
    public int PageSize { get; set; } = 10;   // Default page size
}
```

```
public class PaginationResponseDTO<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public List<T> Items { get; set; }
}
```

---
#### API Response Envelope
- DTO: `ApiResponseDTO<T>`
- Purpose: Standardize API responses for better frontend and integration consistency.

```
public class ApiResponseDTO<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
    public int? StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

---
## Services
- Services handle **application business logic** and **orchestrate operations**.
- Services should be **stateless** and **only depend on repositories or other services**.
- Each main model should have its corresponding Service and Service Interface.

### Responsibilities
- Perform **validations** before calling the database.
- Handle **rules** and **restrictions** (e.g., cannot create a Workout without an Instructor).
- Combine **multiple repositories** when necessary.
- Handle **transactions** (possibly via UnitOfWork).

### System's Services
#### Delete Service
- Interface: IDeletionValidationService
- Purpose: Ensure safe deletion by verifying if an entity is not being referenced elsewhere.

```
public interface IDeletionValidationService
{
    Task<bool> CanDeleteTypeAsync(int typeId);
    Task<bool> CanDeleteModalityAsync(int modalityId);
    Task<bool> CanDeleteHashtagAsync(int hashtagId);
}
```

---
### Services Interfaces
- `IUserService`

```
public interface IUserService : IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>
{
    Task AddGoalToUserAsync(int userId, int goalId);
    Task RemoveGoalFromUserAsync(int userId, int goalId);
    Task<PaginationResponseDTO<GoalOutputDTO>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);

    Task AddInstructorToUserAsync(int userId, int instructorId);
    Task RemoveInstructorFromUserAsync(int userId, int instructorId);
    Task<PaginationResponseDTO<InstructorOutputDTO>> GetInstructorsByUserIdAsync(int userId, PaginationRequestDTO pagination);

	Task AddWorkoutToUserAsync(int userId, int workoutId);
	Task RemoveWorkoutFromUserAsync(int userId, int workoutId)
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
}
```

- `IInstructorService`

```
public interface IInstructorService : IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>
{
	
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
}

```

- `IGoalService`

```
public interface IGoalService : IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ILevelService`

```
public interface ILevelService : IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ITypeService`

```
public interface ITypeService : IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IModalityService`

```
public interface IModalityService : IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IHashtagService`

```
public interface IHashtagService : IGenericService<CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IWorkoutService`

```
public interface IWorkoutService : IGenericInstructorOwnedService<CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>
{
    Task AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId);
    Task RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId);

    Task AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId);
    Task RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId);

    Task AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId);
    Task RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId);

    Task AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId);
    Task RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId);

    Task AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId);
    Task RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId);

    Task AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId);
    Task RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId);

    Task<InstructorOutputDTO> GetInstructorByWorkoutIdAsync(int workoutId);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IRoutineService`

```
public interface IRoutineService : IGenericInstructorOwnedService<CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>
{
    Task AddGoalToRoutineAsync(int routineId, int goalId, int instructorId);
    Task RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId);

    Task AddTypeToRoutineAsync(int routineId, int typeId, int instructorId);
    Task RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId);

    Task AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId);
    Task RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId);

    Task AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId);
    Task RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId);

    Task AddExerciseToRoutineAsync(int routineId, int exerciseId, int order, int sets, int reps, TimeSpan restTime, string note, int day, int week, bool isOptional);
    Task RemoveExerciseFromRoutineAsync(int routineId, int exerciseId);

    Task<InstructorOutputDTO> GetInstructorByRoutineIdAsync(int routineId);
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IExerciseService`

```
public interface IExerciseService : IGenericInstructorOwnedService<CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO>
{
    Task AddGoalToExerciseAsync(int exerciseId, int goalId, int instructorId);
    Task RemoveGoalFromExerciseAsync(int exerciseId, int goalId, int instructorId);

    Task AddTypeToExerciseAsync(int exerciseId, int typeId, int instructorId);
    Task RemoveTypeFromExerciseAsync(int exerciseId, int typeId, int instructorId);

    Task AddModalityToExerciseAsync(int exerciseId, int modalityId, int instructorId);
    Task RemoveModalityFromExerciseAsync(int exerciseId, int modalityId, int instructorId);

    Task AddHashtagToExerciseAsync(int exerciseId, int hashtagId, int instructorId);
    Task RemoveHashtagFromExerciseAsync(int exerciseId, int hashtagId, int instructorId);

    Task AddVariationToExerciseAsync(int exerciseId, int variationId, int instructorId);
    Task RemoveVariationFromExerciseAsync(int exerciseId, int variationId, int instructorId);

    Task<InstructorOutputDTO> GetInstructorByExerciseIdAsync(int exerciseId);
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IUserHasGoalService`

```
public interface IUserHasGoalService
{
    Task<UserHasGoalOutputDTO> CreateUserHasGoalAsync(CreateUserHasGoalInputDTO dto);
    Task DeleteUserHasGoalAsync(int userId, int goalId);

    Task<PaginationResponseDTO<GoalOutputDTO>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByGoalIdAsync(int goalId, PaginationRequestDTO pagination);
}
```

- `IWorkoutHasUserService`

```
public interface IWorkoutHasUserService
{
    Task<WorkoutHasUserOutputDTO> CreateWorkoutHasUserAsync(CreateWorkoutHasUserInputDTO dto);
    Task DeleteWorkoutHasUserAsync(int workoutId, int userId);

    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination);
}
```

- `IRoutineHasExerciseService`

```
public interface IRoutineHasExerciseService
{
    Task<RoutineHasExerciseOutputDTO> CreateRoutineHasExerciseAsync(CreateRoutineHasExerciseInputDTO dto);
    Task DeleteRoutineHasExerciseAsync(int routineId, int exerciseId);

    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByExerciseIdAsync(int exerciseId, PaginationRequestDTO pagination);
}
```

### Services Implementation
- UserService
- InstructorService
- GoalService
- LevelService
- TypeService
- ModalityService
- HashtagService
- WorkoutService:
- RoutineService
- ExerciseService:
- UserHasGoalService
- WorkoutHasUserService
- RoutineHasExerciseService

---
## AutoMapper Profiles
| Entity             | Profile                   |
| ------------------ | ------------------------- |
| User               | UserProfile               |
| Instructor         | InstructorProfile         |
| Goal               | GoalProfile               |
| Level              | LevelProfile              |
| Type               | TypeProfile               |
| Modality           | ModalityProfile           |
| Hashtag            | HashtagProfile            |
| Workout            | WorkoutProfile            |
| Routine            | RoutineProfile            |
| Exercise           | ExerciseProfile           |
| RoutineHasExercise | RoutineHasExerciseProfile |

---
## UnitOfWork Class
The `UnitOfWork` class is the concrete implementation of `IUnitOfWork`.
- Manages a single instance of DbContext.
- Manages a single database transaction.
- Ensures that all changes across multiple repositories are committed or rolled back atomically.
- Provides an integration point to set database session variables (e.g., `SET @user_id = ...`) during a transaction.

**Benefits:**
- Promotes clean, scalable, and maintainable data access.
- Avoids inconsistent data states.
- Provides better transaction management and control.
- Centralizes access to all repositories through a single object.

```
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction;
    private int? _currentUserId;

    public IUserRepository Users { get; private set; }
    public IInstructorRepository Instructors { get; private set; }
    public IGoalRepository Goals { get; private set; }
    public ILevelRepository Levels { get; private set; }
    public ITypeRepository Types { get; private set; }
    public IModalityRepository Modalities { get; private set; }
    public IHashtagRepository Hashtags { get; private set; }
    public IWorkoutRepository Workouts { get; private set; }
    public IRoutineRepository Routines { get; private set; }
    public IExerciseRepository Exercises { get; private set; }
    public IRoutineHasExerciseRepository RoutineHasExercises { get; private set; }

    public UnitOfWork(AppDbContext context,
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

    /// <summary>
    /// Sets the user ID for the current transaction context.
    /// Must be called before any Create, Update, or Delete operations.
    /// </summary>
    public async Task BeginTransactionAsync(int userId)
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync();

        _currentUserId = userId;

        // Sets the @user_id variable inside the MySQL session
        await _context.Database.ExecuteSqlRawAsync($"SET @user_id = {_currentUserId};");
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            _currentUserId = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        _currentUserId = null;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

> Every Create, Update or Delete operation must be wrapped with a `BeginTransactionAsync(currentUserId)` followed by `CommitAsync()`.

Usage on services:

```
await _unitOfWork.BeginTransactionAsync(currentUserId);

await _unitOfWork.Users.AddAsync(newUser);
await _unitOfWork.Goals.AddAsync(newGoal);

await _unitOfWork.CommitAsync();
```