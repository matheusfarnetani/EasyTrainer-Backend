# Domain
## Domain Principles
- Entities must represent real business concepts with identity and behavior.
- Entities should be independent of any external frameworks (e.g., EF Core, Web libraries).
- Only domain-related logic belongs here — no database concerns, no API-specific code.
- Relationships are represented by navigation properties or dedicated relationship models.

---
## Domain Models Summary
| Model Name           | Type               | Notes                                                |
| :------------------- | :----------------- | :--------------------------------------------------- |
| User                 | Main Model         | Represents users of the system                       |
| Instructor           | Main Model         | Represents instructors who create content            |
| Goal                 | Main Model         | Represents fitness goals                             |
| Level                | Main Model         | Represents difficulty levels                         |
| Type                 | Main Model         | Represents exercise/workout types                    |
| Modality             | Main Model         | Represents training modalities                       |
| Hashtag              | Main Model         | Represents tags for workouts and exercises           |
| Workout              | Main Model         | Represents a workout program                         |
| Routine              | Main Model         | Represents a workout routine inside a workout        |
| Exercise             | Main Model         | Represents an individual exercise                    |
| UserHasGoal          | Relationship Model | Links users and goals                                |
| UserHasInstructor    | Relationship Model | Links users and instructors                          |
| WorkoutHasType       | Relationship Model | Links workouts and types                             |
| WorkoutHasModality   | Relationship Model | Links workouts and modalities                        |
| WorkoutHasGoal       | Relationship Model | Links workouts and goals                             |
| WorkoutHasHashtag    | Relationship Model | Links workouts and hashtags                          |
| WorkoutHasUser       | Relationship Model | Links workouts and users                             |
| WorkoutHasRoutine    | Relationship Model | Links workouts and routines                          |
| WorkoutHasExercise   | Relationship Model | Links workouts and exercises                         |
| RoutineHasType       | Relationship Model | Links routines and types                             |
| RoutineHasModality   | Relationship Model | Links routines and modalities                        |
| RoutineHasGoal       | Relationship Model | Links routines and goals                             |
| RoutineHasExercise   | Relationship Model | Links routines and exercises (with extra attributes) |
| RoutineHasHashtag    | Relationship Model | Links routines and hashtags                          |
| ExerciseHasType      | Relationship Model | Links exercises and types                            |
| ExerciseHasModality  | Relationship Model | Links exercises and modalities                       |
| ExerciseHasGoal      | Relationship Model | Links exercises and goals                            |
| ExerciseHasHashtag   | Relationship Model | Links exercises and hashtags                         |
| ExerciseHasVariation | Relationship Model | Links exercises and their variations                 |

---
## Main Models
### User
**Properties**
- Id *int*
- Name *string*  
- Email *string*  
- MobileNumber *string*  
- Birthday *date*  
- Weight *float*  
- Height *float*  
- Gender *char*  
- Password *string*
- LevelId *int*

**Navigation Properties**
- ICollection of UserHasInstructor - UserInstructor
- ICollection of UserHasGoal - UserGoals
- ICollection of WorkoutHasUser - WorkoutUsers

---
### Instructor
**Properties**
- Id *int*
- Name *string*  
- Email  *string*  
- MobileNumber *string*  
- Birthday *date*  
- Gender *char*  
- Password *string*

**Navigation Properties**
- ICollection of UserHasInstructor - UserInstructor
- ICollection of Workout - Workouts
- ICollection of Routine - Routines
- ICollection of Exercise - Exercises

---
### Goal
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of UserHasGoal - UserGoals
- ICollection of WorkoutHasGoal - WorkoutGoals
- ICollection of RoutineHasGoal - RoutineGoals
- ICollection of ExerciseHasGoal - ExerciseGoals

---
### Level
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of User - Users
- ICollection of Workout - Workouts
- ICollection of Routine - Routines
- ICollection of Exercise - Exercises

---
### Type
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of WorkoutHasType - WorkoutTypes
- ICollection of RoutineHasType - RoutineTypes
- ICollection of ExerciseHasType - ExerciseTypes

---
### Modality
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of WorkoutHasModality - WorkoutModalities
- ICollection of RoutineHasModality - RoutineModalities
- ICollection of ExerciseHasModality - ExerciseModalities

---
### Hashtag
**Properties**
- Id *int*
- Hashtag *string*

**Navigation Properties**
- ICollection of WorkoutHasHashtag - WorkoutHashtags
- ICollection of RoutineHasHashtag - RoutineHashtags
- ICollection of ExerciseHasHashtag - ExerciseHashtags

---
### Workout
**Properties**
- Id *int*
- Name *string*  
- Description *string*  
- NumberOfDays *int*  
- ImageUrl *string*  
- Duration *time*  
- Indoor *bool*
- InstructorId *int*
- LevelId *int*

**Navigation Properties**
- ICollection of WorkoutHasType - WorkoutTypes
- ICollection of WorkoutHasModality - WorkoutModalities
- ICollection of WorkoutHasGoal - WorkoutGoals
- ICollection of WorkoutHasHashtag - WorkoutHashtags
- ICollection of WorkoutHasUser - WorkoutUsers
- ICollection of WorkoutHasRoutine - WorkoutRoutines
- ICollection of WorkoutHasExercise - WorkoutExercises

---
### Routine
**Properties**
- Id *int*
- Name *string*  
- Description *string*  
- Duration *time*  
- ImageUrl *string*
- InstructorId *int*
- LevelId *int*

**Navigation Properties**
- ICollection of RoutineHasType - RoutineTypes
- ICollection of RoutineHasModality - RoutineModalities
- ICollection of RoutineHasGoal - RoutineGoals
- ICollection of RoutineHasHashtag - RoutineHashtags
- ICollection of RoutineHasExercise - RoutineExercises
- ICollection of WorkoutHasRoutine - WorkoutRoutines

---
### Exercise
**Properties**
- Id *int*
- Name *string*  
- Description *string*  
- Equipment *string*  
- Duration *time*  
- Repetition *int*  
- Sets *int*  
- RestTime *time*  
- BodyPart *string*  
- VideoUrl *string*  
- ImageUrl *string*  
- Steps *string*  
- Contraindications *string*  
- SafetyTips *string*  
- CommonMistakes *string*  
- IndicatedFor *string*  
- CaloriesBurnedEstimate *float*
- InstructorId *int*
- LevelId *int*

**Navigation Properties**
- ICollection of ExerciseHasType - ExerciseTypes
- ICollection of ExerciseHasModality - ExerciseModalities
- ICollection of ExerciseHasGoal - ExerciseGoals
- ICollection of ExerciseHasHashtag - ExerciseHashtags
- ICollection of ExerciseHasVariation - ExerciseVariations
- ICollection of RoutineHasExercise - RoutineExercises        
- ICollection of WorkoutHasExercise - WorkoutExercises

---
## Relationship Models
All relationship models follow the naming convention: `EntityOneHasEntityTwo` and each relationship model has two navigation properties.

### UserHasInstructor
**Properties**
- UserId *int*  
- InstructorId *int*

**Navigation Properties**
- User
- Instructor

---
### UserHasGoal
**Properties**
- UserId *int*  
- GoalId *int*

**Navigation Properties**
- User
- Goal

---
### WorkoutHasType
**Properties**
- WorkoutId *int*  
- TypeId *int*

**Navigation Properties**
- Workout
- Type

---
### WorkoutHasModality
**Properties**
- WorkoutId *int*  
- ModalityId *int*

**Navigation Properties**
- Workout
- Modality

---
### WorkoutHasGoal
**Properties**
- WorkoutId *int*  
- GoalId *int*

**Navigation Properties**
- Workout
- Goal

---
### WorkoutHasHashtag
**Properties**
- WorkoutId *int*  
- HashtagId *int*

**Navigation Properties**
- Workout
- Hashtag

---
### WorkoutHasUser
**Properties**
- WorkoutId  *int*
- UserId *int*

**Navigation Properties**
- Workout
- User

---
### WorkoutHasRoutine
**Properties**
- WorkoutId  *int*
- RoutineId *int*

**Navigation Properties**
- Workout
- Routine

---
### WorkoutHasExercise
**Properties**
- WorkoutId  *int*
- ExerciseId *int*

**Navigation Properties**
- Workout
- Exercise

---
### RoutineHasType
**Properties**
- RoutineId *int*  
- TypeId *int*

**Navigation Properties**
- Routine
- Type

---
### RoutineHasModality
**Properties**
- RoutineId *int*  
- ModalityId *int*

**Navigation Properties**
- Routine
- Modality

---
### RoutineHasGoal
**Properties**
- RoutineId *int*  
- GoalId *int*

**Navigation Properties**
- Routine
- Goal

---
### RoutineHasExercise
**Properties**
- RoutineId *int*  
- ExerciseId *int*  
- Order *int*  
- Sets *int*  
- Reps *int*  
- RestTime *time*  
- Note *string*  
- Day *int*  
- Week *int*  
- IsOptional *bool*

**Navigation Properties**
- Routine
- Exercise

---
### RoutineHasHashtag
**Properties**
- RoutineId *int*  
- HashtagId *int*

**Navigation Properties**
- Routine
- Hashtag

---
### ExerciseHasType
**Properties**
- ExerciseId *int*  
- TypeId *int*

**Navigation Properties**
- Exercise
- Type

---
### ExerciseHasModality
**Properties**
- ExerciseId *int*  
- ModalityId *int*

**Navigation Properties**
- Exercise
- Modality

---
### ExerciseHasGoal
**Properties**
- ExerciseId *int*  
- GoalId *int*

**Navigation Properties**
- Exercise
- Goal

---
### ExerciseHasHashtag
**Properties**
- ExerciseId *int*  
- HashtagId *int*

**Navigation Properties**
- Exercise
- Hashtag

---
### ExerciseHasVariation
**Properties**
- ExerciseId *int*  
- VariationId *int*

**Navigation Properties**
- Exercise
- Variation

---
## Value Objects (Optional for future)
- Value Objects represent attributes that do not have identity.
- Currently not used, but examples could include `Address`, `ContactInformation`, etc., if necessary in future domain expansions.

---
## Repositories Interfaces

### Generic Repository
The **Generic Repository Pattern** is designed to provide a reusable set of data operations (CRUD) for different entities without duplicating code.

The logic behind it is:
- Define a **generic interface** `IGenericRepository` that works with any entity type.
- Implement **standard CRUD operations** once, and reuse them for all entities.
- Extend the generic repository with **specific repositories** if you need custom queries (e.g., `GetUserByEmail`, `GetWorkoutsByUserId`).

**Benefits of using a Generic Repository:**
- Reduces code duplication by centralizing common CRUD operations.
- Ensures consistency across all data access layers.
- Simplifies maintenance by isolating data access logic.
- Facilitates scalability when adding new entities.
- Promotes separation of concerns between business logic and data persistence.

---
### IGenericRepository

```
public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T?>> GetAllAsync(bool tracked = true);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task SaveAsync();
    }
```

All main domain models have their own repository and repository interface, following the generic repository pattern.

---
### Main Repositories Interfaces
- `IUserRepository`

```
public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId);
    Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId);
    Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId);
    Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId);
}
```

- `IInstructorRepository`

```
public interface IInstructorRepository : IGenericRepository<Instructor>
{
    Task<Instructor> GetInstructorByEmailAsync(string email);
    Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId);
    Task<Instructor> GetInstructorByWorkoutIdAsync(int workoutId);
    Task<Instructor> GetInstructorByRoutineIdAsync(int routineId);
    Task<Instructor> GetInstructorByExerciseIdAsync(int exerciseId);
}
```

- `IGoalRepository`

```
public interface IGoalRepository : IGenericRepository<Goal>
{
    Task<IEnumerable<Goal>> GetGoalsByUserAsync(int userId);
    Task<IEnumerable<Goal>> GetGoalsByWorkoutAsync(int workoutId);
    Task<IEnumerable<Goal>> GetGoalsByRoutineAsync(int routineId);
    Task<IEnumerable<Goal>> GetGoalsByExerciseAsync(int exerciseId);
}
```


- `ILevelRepository`

```
public interface ILevelRepository : IGenericRepository<Level>
{
    Task<Level> GetLevelByUserAsync(int userId);
    Task<Level> GetLevelByWorkoutAsync(int workoutId);
    Task<Level> GetLevelByRoutineAsync(int routineId);
    Task<Level> GetLevelByExerciseAsync(int exerciseId);
}
```


- `ITypeRepository`

```
public interface ITypeRepository : IGenericRepository<Type>
{
    Task<IEnumerable<Type>> GetTypesByWorkoutAsync(int workoutId);
    Task<IEnumerable<Type>> GetTypesByRoutineAsync(int routineId);
    Task<IEnumerable<Type>> GetTypesByExerciseAsync(int exerciseId);
}
```


- `IModalityRepository`

```
public interface IModalityRepository : IGenericRepository<Modality>
{
    Task<IEnumerable<Modality>> GetModalitiesByWorkoutAsync(int workoutId);
    Task<IEnumerable<Modality>> GetModalitiesByRoutineAsync(int routineId);
    Task<IEnumerable<Modality>> GetModalitiesByExerciseAsync(int exerciseId);
}
```


- `IHashtagRepository`

```
public interface IHashtagRepository : IGenericRepository<Hashtag>
{
    Task<IEnumerable<Hashtag>> GetHashtagsByWorkoutAsync(int workoutId);
    Task<IEnumerable<Hashtag>> GetHashtagsByRoutineAsync(int routineId);
    Task<IEnumerable<Hashtag>> GetHashtagsByExerciseAsync(int exerciseId);
}
```


- `IWorkoutRepository`

```
public interface IWorkoutRepository : IGenericRepository<Workout>
{
	Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
	Task<IEnumerable<Workout>> GetWorkoutsByInstructorIdAsync(int instructorId);
	Task<IEnumerable<Workout>> GetWorkoutsByGoalIdAsync(int goalId, int? instructorId = null, int? userId = null);
	Task<IEnumerable<Workout>> GetWorkoutsByLevelIdAsync(int levelId, int? instructorId = null, int? userId = null);
	Task<IEnumerable<Workout>> GetWorkoutsByModalityAsync(int modalityId, int? instructorId = null, int? userId = null);
	Task<IEnumerable<Workout>> GetWorkoutsByHashtagAsync(int hashtagId, int? instructorId = null, int? userId = null);
	Task<IEnumerable<Workout>> GetWorkoutsByRoutineAsync(int routineId, int? instructorId = null, int? userId = null);
	Task<IEnumerable<Workout>> GetWorkoutsByExerciseAsync(int exerciseId, int? instructorId = null, int? userId = null);
}
```


- `IRoutineRepository`

```
public interface IRoutineRepository : IGenericRepository<Routine>
{
	Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int workoutId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByModalityAsync(int modalityId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByHashtagAsync(int hashtagId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByWorkoutAsync(int workoutId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByExerciseAsync(int exerciseId, int instructorId);
}
```


- `IExerciseRepository`

```
public interface IExerciseRepository : IGenericRepository<Exercise>
{
	Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int workoutId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByModalityAsync(int modalityId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByHashtagAsync(int hashtagId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByRoutineAsync(int routineId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByExerciseAsync(int exerciseId, int instructorId);
	Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int variationId, int instructorId);
}
```

---
### Relationship Repositories Interfaces
For relationships containing additional attributes, we define specific repositories.
- `IRoutineHasExerciseRepository`

```
public interface IRoutineHasExerciseRepository : IGenericRepository<RoutineHasExercise>
{
	Task<IEnumerable<RoutineHasExercise>> GetExercisesByRoutineIdAsync(int routineId);
	Task<IEnumerable<RoutineHasExercise>> GetRoutinesByExerciseIdAsync(int exerciseId);
}
```

---
### Notes
- Only `RoutineHasExercise` has a dedicated repository because it contains additional business attributes (order, sets, reps, rest time, notes, day, week, is_optional).
- All other relationship tables (simple many-to-many links) are handled through entity navigation and do not require separate repositories.

---
## Unit of Work (UoW)
The Unit of Work pattern is used to group multiple database operations into a single transaction.

In this project, the Unit of Work also manages the database session variable `@user_id` to properly record the user responsible for each change.

### Key Features
- **BeginTransactionAsync(userId)**: Starts a transaction and sets the `@user_id` in the MySQL session.
- **CommitAsync()**: Commits all changes atomically; if an error occurs, automatically rolls back.
- **RollbackAsync()**: Cancels all pending changes if necessary.
- **Repositories Access**: Provides access to all repositories through properties.

---
### Why use this approach?
- Ensures **data consistency**: Either all changes are saved or none.
- Centralizes **user context** for auditing operations.
- Reduces duplication of transaction management across services.

---
### IUnitOfWork
The `IUnitOfWork` interface defines the structure for the Unit of Work:
- Exposes access to all the repositories.
- Allows starting a transaction (`BeginTransactionAsync`).
- Allows committing all changes (`CommitAsync`).
- Allows rolling back changes in case of failure (`RollbackAsync`).
- Implements IDisposable to manage the DbContext lifetime.

```
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
```

---
## IConnectionManager Interface
The `IConnectionManager` interface defines the contract for retrieving database connection strings based on user roles.

This abstraction allows the application to dynamically determine which database credentials should be used for a given user context without tying the business logic to infrastructure-specific details.

```
public interface IConnectionManager
{
    string GetConnectionString(string role);
}
```

```
{
  "ConnectionStrings": {
    "EasyTrainerAdmin": "Server=localhost;Database=easytrainer;User=easytrainer_admin;Password=adminpassword;",
    "EasyTrainerInstructor": "Server=localhost;Database=easytrainer;User=easytrainer_instructor;Password=instructorpassword;",
    "EasyTrainerUser": "Server=localhost;Database=easytrainer;User=easytrainer_user;Password=userpassword;"
  }
}
```

---
## Generic Services Interface
To promote **code reuse**, **clean architecture**, and **scalability** within the Application Layer, EasyTrainer adopts two types of **Generic Services**:
- `IGenericService`: For **simple entities** not linked to a specific Instructor.
- `IGenericInstructorOwnedService`: For **Instructor-owned entities** like `Workouts`, `Routines`, and `Exercises`.
    
This design follows **DRY** (Don't Repeat Yourself) and **Single Responsibility Principle** from SOLID principles.

### IGenericService
Handles standard CRUD operations for simple models.

```
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
### IGenericInstructorOwnedService
**Use case:**  
For entities that **must be owned** and **controlled by an Instructor**, such as `Workout`, `Routine`, and `Exercise`.

**Interface:**

```
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
## Delete Service Interface
### IDeletionValidationService

```
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
