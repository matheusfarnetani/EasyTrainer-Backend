# Application
## DTOs
- Data Transfer Objects (DTOs) are simple objects used to encapsulate data sent between the API and the application layer.
- Separate **Input DTOs** and **Output DTOs** for each Main Model.

### Inputs
- DTOs used for **Create** and **Update** operations.
- Contain only fields allowed to be written by the client.
- May include validation annotations (e.g., `[Required]`, `[MaxLength]`).
    - `CreateUserInputDTO`
    - `UpdateUserInputDTO`
    - `CreateInstructorInputDTO`
    - `UpdateInstructorInputDTO`
    - `CreateGoalInputDTO`
    - `UpdateGoalInputDTO`
    - `CreateLevelInputDTO`
    - `UpdateLevelInputDTO`
    - `CreateTypeInputDTO`
    - `UpdateTypeInputDTO`
    - `CreateModalityInputDTO`
    - `UpdateModalityInputDTO`
    - `CreateHashtagInputDTO`
    - `UpdateHashtagInputDTO`
    - `CreateWorkoutInputDTO`
    - `UpdateWorkoutInputDTO`
    - `CreateRoutineInputDTO`
    - `UpdateRoutineInputDTO`
    - `CreateExerciseInputDTO`
    - `UpdateExerciseInputDTO`
    - `CreateUserHasGoalInputDTO`
    - `CreateWorkoutHasUserInputDTO`
    - `CreateRoutineHasExerciseInputDTO`

---
### Outputs
- DTOs used for **Read** operations.
- May expose additional computed fields (e.g., `FullName`, `GoalNames`, etc.).
- Never expose sensitive fields (e.g., passwords).
    - `UserOutputDTO`
    - `InstructorOutputDTO`
    - `GoalOutputDTO`
    - `LevelOutputDTO`
    - `TypeOutputDTO`
    - `ModalityOutputDTO`
    - `HashtagOutputDTO`
    - `WorkoutOutputDTO`
    - `RoutineOutputDTO`
    - `ExerciseOutputDTO`
    - `VariationOutputDTO`
    - `UserHasGoalOutputDTO`
    - `WorkoutHasUserOutputDTO`
    - `RoutineHasExerciseOutputDTO`

---
### System's DTOs
#### Pagination
- DTOs: `PaginationRequestDTO`, `PaginationResponseDTO<T>`
- Purpose: Improve scalability by supporting paging in list operations.

```csharp
public class PaginationRequestDTO
{
	private int _page = 1;
	private int _pageSize = 10;
	public int Page
	{
		get => _page;
		set => _page = (value <= 0) ? 1 : value;
	}
	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = (value <= 0) ? 10 : (value > 100 ? 100 : value);
	}
	public string? Search { get; set; }
	public string? OrderBy { get; set; }
	public bool Ascending { get; set; } = true;
}
```

```csharp
public class PaginationResponseDTO<T>
{
	public List<T> Data { get; set; } = new List<T>();
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int TotalCount { get; set; }
	public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
```

---
#### Service Response Envelope
- DTO: `ServiceResponseDTO<T>`
- Purpose: Standardize API responses for better frontend and integration consistency.

```csharp
public class ServiceResponseDTO<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int? StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ServiceResponseDTO<T> CreateSuccess(T data, string message = "Success")
    {
        return new ServiceResponseDTO<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResponseDTO<T> CreateFailure(string message, List<string>? errors = null, int? statusCode = null)
    {
        return new ServiceResponseDTO<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode
        };
    }
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
- Handle **transactions** (possibly via `UnitOfWork`).

### Generic Services Interface
To promote **code reuse**, **clean architecture**, and **scalability** within the Application Layer, EasyTrainer adopts two types of **Generic Services**:
- `IGenericService`: For **simple entities** not linked to a specific Instructor.
- `IGenericInstructorOwnedService`: For **Instructor-owned entities** like `Workouts`, `Routines`, and `Exercises`.
    
This design follows **DRY** (Don't Repeat Yourself) and **Single Responsibility Principle** from SOLID principles.

#### IGenericService
Handles standard CRUD operations for simple models.

```csharp
public interface IGenericService<TCreateDTO, TUpdateDTO, TOutputDTO>
{
    Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto);
    Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto);
    Task<ServiceResponseDTO<bool>> DeleteAsync(int id);
    Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id);
    Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(PaginationRequestDTO pagination);
}
```

**Responsibilities:**
- Handle basic Create, Update, Delete, Get by ID, and Paginated List operations.
- Abstracts common CRUD behavior into a shared, reusable interface.

|Service|Interface|
|---|---|
|IUserService|IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>|
|IInstructorService|IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>|
|IGoalService|IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>|
|ITypeService|IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>|
|ILevelService|IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>|
|IModalityService|IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>|
|IHashtagService|IGenericService<CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>|

---
#### IGenericInstructorOwnedService
**Use case:**  
For entities that **must be owned** and **controlled by an Instructor**, such as `Workout`, `Routine`, and `Exercise`.

**Interface:**

```csharp
public interface IGenericInstructorOwnedService<TCreateDTO, TUpdateDTO, TOutputDTO>
{
    Task<ServiceResponseDTO<TOutputDTO>> CreateAsync(TCreateDTO dto, int instructorId);
    Task<ServiceResponseDTO<TOutputDTO>> UpdateAsync(TUpdateDTO dto, int instructorId);
    Task<ServiceResponseDTO<bool>> DeleteAsync(int id, int instructorId);
    Task<ServiceResponseDTO<TOutputDTO>> GetByIdAsync(int id, int instructorId);
    Task<ServiceResponseDTO<PaginationResponseDTO<TOutputDTO>>> GetAllAsync(int instructorId, PaginationRequestDTO pagination);
}
```

**Responsibilities:**
- Same as `IGenericService`, but every operation explicitly receives the `instructorId` to enforce ownership.
- Adds security and control, ensuring that instructors can only manage their own resources.

| Service          | Interface                                                                                         |
| ---------------- | ------------------------------------------------------------------------------------------------- |
| IWorkoutService  | IGenericInstructorOwnedService<CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>    |
| IRoutineService  | IGenericInstructorOwnedService<CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>    |
| IExerciseService | IGenericInstructorOwnedService<CreateExerciseInputDTO, UpdateExerciseInputDTO, ExerciseOutputDTO> |

---
### Delete Service
#### IDeletionValidationService

```csharp
public interface IDeletionValidationService
{
    Task<bool> CanDeleteTypeAsync(int typeId, int instructorId);
    Task<bool> CanDeleteModalityAsync(int modalityId, int instructorId);
    Task<bool> CanDeleteHashtagAsync(int hashtagId, int instructorId);
    Task<bool> CanDeleteGoalAsync(int goalId, int instructorId);
}
```

|Item|Description|
|---|---|
|Purpose|Declares the validation logic that must be followed before deleting entities.|
|Responsibility|Domain defines the rules but does not know how they are implemented.|
|Entities Covered|`Type`, `Modality`, `Hashtag` (entities linked to workouts, routines, exercises).|

---
#### DeletionValidationService
**Purpose:** The `DeletionValidationService` is responsible for ensuring **safe deletions** of auxiliary entities like `Type`, `Modality`, and `Hashtag`.  
It verifies that an entity is **not being referenced** in other related tables (such as `Workouts`, `Routines`, or `Exercises`) before allowing its deletion.

This service helps prevent:
- Broken foreign key relationships
- Data inconsistencies
- System errors after deletions

It enforces **business rules** that protect data integrity.

```csharp
ublic class DeletionValidationService : IDeletionValidationService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IRoutineRepository _routineRepository;
    private readonly IExerciseRepository _exerciseRepository;

    public DeletionValidationService(
        IWorkoutRepository workoutRepository,
        IRoutineRepository routineRepository,
        IExerciseRepository exerciseRepository)
    {
        _workoutRepository = workoutRepository;
        _routineRepository = routineRepository;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<bool> CanDeleteTypeAsync(int typeId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId, instructorId);
        var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteModalityAsync(int modalityId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId, instructorId);
        var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteHashtagAsync(int hashtagId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId, instructorId);
        var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }

    public async Task<bool> CanDeleteGoalAsync(int goalId, int instructorId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByGoalIdAsync(goalId, instructorId);
        var routines = await _routineRepository.GetRoutinesByGoalIdAsync(goalId, instructorId);
        var exercises = await _exerciseRepository.GetExercisesByGoalIdAsync(goalId, instructorId);

        return !workouts.Any() && !routines.Any() && !exercises.Any();
    }
}
```

---
### Services Interfaces
- `IUserService`

```csharp
ublic interface IUserService : IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>
{
    Task<ServiceResponseDTO<UserOutputDTO>> GetByEmailAsync(string email);
    
    // Instructor
    Task AddInstructorToUserAsync(int userId, int instructorId);
    Task RemoveInstructorFromUserAsync(int userId, int instructorId);
    Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetInstructorsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    
    // Level
    Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByLevelIdAsync(int levelId, PaginationRequestDTO pagination);

    // Goal
    Task AddGoalToUserAsync(int userId, int goalId);
    Task RemoveGoalFromUserAsync(int userId, int goalId);
    Task<ServiceResponseDTO<PaginationResponseDTO<GoalOutputDTO>>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByGoalIdAsync(int goalId, PaginationRequestDTO pagination);

    // Workout
    Task AddWorkoutToUserAsync(int userId, int workoutId, int instructorId);
    Task RemoveWorkoutFromUserAsync(int userId, int workoutId, int instructorId);
    Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<ServiceResponseDTO<PaginationResponseDTO<UserOutputDTO>>> GetByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination);
}
```

- `IInstructorService`

```csharp
public interface IInstructorService : IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>
{
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetByEmailAsync(string email);

	// User
	Task<ServiceResponseDTO<bool>> AddUserToInstructorAsync(int instructorId, int userId);
	Task<ServiceResponseDTO<bool>> RemoveUserFromInstructorAsync(int instructorId, int userId);
	Task<ServiceResponseDTO<PaginationResponseDTO<InstructorOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);

	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsAsync(int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetByWorkoutIdAsync(int workoutId);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesAsync(int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetByRoutineIdAsync(int routineId);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesAsync(int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetByExerciseIdAsync(int exerciseId);
}
```

- `IGoalService`

```csharp
public interface IGoalService : IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>
{
	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ILevelService`

```csharp
public interface ILevelService : IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>
{
	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ITypeService`

```csharp
public interface ITypeService : IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>
{
	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IModalityService`

```csharp
public interface IModalityService : IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>
{
	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IHashtagService`

```csharp
public interface IHashtagService : IGenericService<CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>
{
	// Workout
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);

	// Routine
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);

	// Exercise
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IWorkoutService`

```csharp
public interface IWorkoutService : IGenericInstructorOwnedService<CreateWorkoutInputDTO, UpdateWorkoutInputDTO, WorkoutOutputDTO>
{
	// Relationship management: Add/Remove
	Task<ServiceResponseDTO<bool>> AddGoalToWorkoutAsync(int workoutId, int goalId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveGoalFromWorkoutAsync(int workoutId, int goalId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddTypeToWorkoutAsync(int workoutId, int typeId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveTypeFromWorkoutAsync(int workoutId, int typeId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddModalityToWorkoutAsync(int workoutId, int modalityId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveModalityFromWorkoutAsync(int workoutId, int modalityId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddHashtagToWorkoutAsync(int workoutId, int hashtagId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveHashtagFromWorkoutAsync(int workoutId, int hashtagId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddRoutineToWorkoutAsync(int workoutId, int routineId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveRoutineFromWorkoutAsync(int workoutId, int routineId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddExerciseToWorkoutAsync(int workoutId, int exerciseId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveExerciseFromWorkoutAsync(int workoutId, int exerciseId, int instructorId);

	// Retrievals
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByWorkoutIdAsync(int workoutId);
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<RoutineOutputDTO>>> GetRoutinesByWorkoutIdAsync(int workoutId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByUserIdAsync(int userId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
	Task<ServiceResponseDTO<PaginationResponseDTO<WorkoutOutputDTO>>> GetByExerciseIdAsync(int exerciseId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IRoutineService`

```csharp
public interface IRoutineService : IGenericInstructorOwnedService<CreateRoutineInputDTO, UpdateRoutineInputDTO, RoutineOutputDTO>
{
	// Add/Remove Relationships
	Task<ServiceResponseDTO<bool>> AddGoalToRoutineAsync(int routineId, int goalId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveGoalFromRoutineAsync(int routineId, int goalId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddTypeToRoutineAsync(int routineId, int typeId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveTypeFromRoutineAsync(int routineId, int typeId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddModalityToRoutineAsync(int routineId, int modalityId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveModalityFromRoutineAsync(int routineId, int modalityId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddHashtagToRoutineAsync(int routineId, int hashtagId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveHashtagFromRoutineAsync(int routineId, int hashtagId, int instructorId);

	Task<ServiceResponseDTO<bool>> AddExerciseToRoutineAsync(int routineId, int exerciseId, int instructorId);
	Task<ServiceResponseDTO<bool>> RemoveExerciseFromRoutineAsync(int routineId, int exerciseId, int instructorId);

	// Getters
	Task<ServiceResponseDTO<InstructorOutputDTO>> GetInstructorByRoutineIdAsync(int routineId);
	Task<ServiceResponseDTO<PaginationResponseDTO<ExerciseOutputDTO>>> GetExercisesByRoutineIdAsync(int routineId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IExerciseService`

```csharp
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

```csharp
public interface IUserHasGoalService
{
    Task<UserHasGoalOutputDTO> CreateUserHasGoalAsync(CreateUserHasGoalInputDTO dto);
    Task DeleteUserHasGoalAsync(int userId, int goalId);

    Task<PaginationResponseDTO<GoalOutputDTO>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByGoalIdAsync(int goalId, PaginationRequestDTO pagination);
}
```

- `IWorkoutHasUserService`

```csharp
public interface IWorkoutHasUserService
{
    Task<WorkoutHasUserOutputDTO> CreateWorkoutHasUserAsync(CreateWorkoutHasUserInputDTO dto);
    Task DeleteWorkoutHasUserAsync(int workoutId, int userId);

    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByWorkoutIdAsync(int workoutId, PaginationRequestDTO pagination);
}
```

- `IRoutineHasExerciseService`

```csharp
public interface IRoutineHasExerciseService
{
    Task<RoutineHasExerciseOutputDTO> CreateRoutineHasExerciseAsync(CreateRoutineHasExerciseInputDTO dto);
    Task DeleteRoutineHasExerciseAsync(int routineId, int exerciseId);

    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByRoutineIdAsync(int routineId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByExerciseIdAsync(int exerciseId, PaginationRequestDTO pagination);
}
```

### Services Implementation
- `UserService`
- `InstructorService`
- `GoalService`
- `LevelService`
- `TypeService`
- `ModalityService`
- `HashtagService`
- `WorkoutService`
- `RoutineService`
- `ExerciseService`
- `UserHasGoalService`
- `WorkoutHasUserService`
- `RoutineHasExerciseService`

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

