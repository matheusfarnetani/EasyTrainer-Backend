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
    public int PageNumber { get; set; } = 1;  // Default to first page
    public int PageSize { get; set; } = 10;   // Default page size
}
```

```csharp
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

```csharp
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
    Task<TOutputDTO> CreateAsync(TCreateDTO dto);
    Task<TOutputDTO> UpdateAsync(TUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<TOutputDTO> GetByIdAsync(int id);
    Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(PaginationRequestDTO pagination);
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
    Task<TOutputDTO> CreateAsync(TCreateDTO dto, int instructorId);
    Task<TOutputDTO> UpdateAsync(TUpdateDTO dto, int instructorId);
    Task DeleteAsync(int id, int instructorId);
    Task<TOutputDTO> GetByIdAsync(int id, int instructorId);
    Task<PaginationResponseDTO<TOutputDTO>> GetAllAsync(int instructorId, PaginationRequestDTO pagination);
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
    Task<bool> CanDeleteTypeAsync(int typeId);
    Task<bool> CanDeleteModalityAsync(int modalityId);
    Task<bool> CanDeleteHashtagAsync(int hashtagId);
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
public class DeletionValidationService : IDeletionValidationService
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

    public async Task<bool> CanDeleteTypeAsync(int typeId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByTypeIdAsync(typeId);
        var routines = await _routineRepository.GetRoutinesByTypeIdAsync(typeId);
        var exercises = await _exerciseRepository.GetExercisesByTypeIdAsync(typeId);

        return workouts.Count == 0 && routines.Count == 0 && exercises.Count == 0;
    }

    public async Task<bool> CanDeleteModalityAsync(int modalityId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByModalityIdAsync(modalityId);
        var routines = await _routineRepository.GetRoutinesByModalityIdAsync(modalityId);
        var exercises = await _exerciseRepository.GetExercisesByModalityIdAsync(modalityId);

        return workouts.Count == 0 && routines.Count == 0 && exercises.Count == 0;
    }

    public async Task<bool> CanDeleteHashtagAsync(int hashtagId)
    {
        var workouts = await _workoutRepository.GetWorkoutsByHashtagIdAsync(hashtagId);
        var routines = await _routineRepository.GetRoutinesByHashtagIdAsync(hashtagId);
        var exercises = await _exerciseRepository.GetExercisesByHashtagIdAsync(hashtagId);

        return workouts.Count == 0 && routines.Count == 0 && exercises.Count == 0;
    }
	}
```

---
### Services Interfaces
- `IUserService`

```csharp
public interface IUserService : IGenericService<CreateUserInputDTO, UpdateUserInputDTO, UserOutputDTO>
{
    Task AddGoalToUserAsync(int userId, int goalId);
    Task RemoveGoalFromUserAsync(int userId, int goalId);
    Task<PaginationResponseDTO<GoalOutputDTO>> GetGoalsByUserIdAsync(int userId, PaginationRequestDTO pagination);

    Task AddInstructorToUserAsync(int userId, int instructorId);
    Task RemoveInstructorFromUserAsync(int userId, int instructorId);
    Task<PaginationResponseDTO<InstructorOutputDTO>> GetInstructorsByUserIdAsync(int userId, PaginationRequestDTO pagination);

	Task AddWorkoutToUserAsync(int userId, int workoutId);
	Task RemoveWorkoutFromUserAsync(int userId, int workoutId);
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByUserIdAsync(int userId, PaginationRequestDTO pagination);
}
```

- `IInstructorService`

```csharp
public interface IInstructorService : IGenericService<CreateInstructorInputDTO, UpdateInstructorInputDTO, InstructorOutputDTO>
{
	
    Task<PaginationResponseDTO<UserOutputDTO>> GetUsersByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByInstructorIdAsync(int instructorId, PaginationRequestDTO pagination);
}

```

- `IGoalService`

```csharp
public interface IGoalService : IGenericService<CreateGoalInputDTO, UpdateGoalInputDTO, GoalOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByGoalIdAsync(int goalId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ILevelService`

```csharp
public interface ILevelService : IGenericService<CreateLevelInputDTO, UpdateLevelInputDTO, LevelOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByLevelIdAsync(int levelId, int instructorId, PaginationRequestDTO pagination);
}
```

- `ITypeService`

```csharp
public interface ITypeService : IGenericService<CreateTypeInputDTO, UpdateTypeInputDTO, TypeOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByTypeIdAsync(int typeId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IModalityService`

```csharp
public interface IModalityService : IGenericService<CreateModalityInputDTO, UpdateModalityInputDTO, ModalityOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByModalityIdAsync(int modalityId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IHashtagService`

```csharp
public interface IHashtagService : IGenericService<CreateHashtagInputDTO, UpdateHashtagInputDTO, HashtagOutputDTO>
{
    // Relationships
    Task<PaginationResponseDTO<WorkoutOutputDTO>> GetWorkoutsByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<RoutineOutputDTO>> GetRoutinesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
    Task<PaginationResponseDTO<ExerciseOutputDTO>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId, PaginationRequestDTO pagination);
}
```

- `IWorkoutService`

```csharp
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

```csharp
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
