# Domain
## Domain Principles
- Entities must represent real business concepts with identity and behavior.
- Entities should be independent of any external frameworks (e.g., EF Core, Web libraries).
- Only domain-related logic belongs here — no database concerns, no API-specific code.
- Relationships are represented by navigation properties or dedicated relationship models.

---
## Entities
### Entities Summary
| Model Name           | Type               | Notes                                                |
| :------------------- | :----------------- | :--------------------------------------------------- |
| User                 | Main               | Represents users of the system                       |
| Instructor           | Main               | Represents instructors who create content            |
| Goal                 | Main               | Represents fitness goals                             |
| Level                | Main               | Represents difficulty levels                         |
| Type                 | Main               | Represents exercise/workout types                    |
| Modality             | Main               | Represents training modalities                       |
| Hashtag              | Main               | Represents tags for workouts and exercises           |
| Workout              | Main               | Represents a workout program                         |
| Routine              | Main               | Represents a workout routine inside a workout        |
| Exercise             | Main               | Represents an individual exercise                    |
| UserHasGoal          | Relationship       | Links users and goals                                |
| UserHasInstructor    | Relationship       | Links users and instructors                          |
| WorkoutHasType       | Relationship       | Links workouts and types                             |
| WorkoutHasModality   | Relationship       | Links workouts and modalities                        |
| WorkoutHasGoal       | Relationship       | Links workouts and goals                             |
| WorkoutHasHashtag    | Relationship       | Links workouts and hashtags                          |
| WorkoutHasUser       | Relationship       | Links workouts and users                             |
| WorkoutHasRoutine    | Relationship       | Links workouts and routines                          |
| WorkoutHasExercise   | Relationship       | Links workouts and exercises                         |
| RoutineHasType       | Relationship       | Links routines and types                             |
| RoutineHasModality   | Relationship       | Links routines and modalities                        |
| RoutineHasGoal       | Relationship       | Links routines and goals                             |
| RoutineHasExercise   | Relationship       | Links routines and exercises (with extra attributes) |
| RoutineHasHashtag    | Relationship       | Links routines and hashtags                          |
| ExerciseHasType      | Relationship       | Links exercises and types                            |
| ExerciseHasModality  | Relationship       | Links exercises and modalities                       |
| ExerciseHasGoal      | Relationship       | Links exercises and goals                            |
| ExerciseHasHashtag   | Relationship       | Links exercises and hashtags                         |
| ExerciseHasVariation | Relationship       | Links exercises and their variations                 |

---
### Main
#### User
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
#### Instructor
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
#### Goal
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
#### Level
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
#### TrainingType
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of WorkoutHasType - WorkoutTypes
- ICollection of RoutineHasType - RoutineTypes
- ICollection of ExerciseHasType - ExerciseTypes

---
#### Modality
**Properties**
- Id *int*
- Name *string*  
- Description *string*

**Navigation Properties**
- ICollection of WorkoutHasModality - WorkoutModalities
- ICollection of RoutineHasModality - RoutineModalities
- ICollection of ExerciseHasModality - ExerciseModalities

---
#### Hashtag
**Properties**
- Id *int*
- Hashtag *string*

**Navigation Properties**
- ICollection of WorkoutHasHashtag - WorkoutHashtags
- ICollection of RoutineHasHashtag - RoutineHashtags
- ICollection of ExerciseHasHashtag - ExerciseHashtags

---
#### Workout
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
#### Routine
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
#### Exercise
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
### Relationship 
All relationship entities follow the naming convention: `EntityOneHasEntityTwo` and each relationship model has two navigation properties.

#### UserHasInstructor
**Properties**
- UserId *int*  
- InstructorId *int*

**Navigation Properties**
- User
- Instructor

---
#### UserHasGoal
**Properties**
- UserId *int*  
- GoalId *int*

**Navigation Properties**
- User
- Goal

---
#### WorkoutHasType
**Properties**
- WorkoutId *int*  
- TypeId *int*

**Navigation Properties**
- Workout
- Type

---
#### WorkoutHasModality
**Properties**
- WorkoutId *int*  
- ModalityId *int*

**Navigation Properties**
- Workout
- Modality

---
#### WorkoutHasGoal
**Properties**
- WorkoutId *int*  
- GoalId *int*

**Navigation Properties**
- Workout
- Goal

---
#### WorkoutHasHashtag
**Properties**
- WorkoutId *int*  
- HashtagId *int*

**Navigation Properties**
- Workout
- Hashtag

---
#### WorkoutHasUser
**Properties**
- WorkoutId  *int*
- UserId *int*

**Navigation Properties**
- Workout
- User

---
#### WorkoutHasRoutine
**Properties**
- WorkoutId  *int*
- RoutineId *int*

**Navigation Properties**
- Workout
- Routine

---
#### WorkoutHasExercise
**Properties**
- WorkoutId  *int*
- ExerciseId *int*

**Navigation Properties**
- Workout
- Exercise

---
#### RoutineHasType
**Properties**
- RoutineId *int*  
- TypeId *int*

**Navigation Properties**
- Routine
- Type

---
#### RoutineHasModality
**Properties**
- RoutineId *int*  
- ModalityId *int*

**Navigation Properties**
- Routine
- Modality

---
#### RoutineHasGoal
**Properties**
- RoutineId *int*  
- GoalId *int*

**Navigation Properties**
- Routine
- Goal

---
#### RoutineHasExercise
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
#### RoutineHasHashtag
**Properties**
- RoutineId *int*  
- HashtagId *int*

**Navigation Properties**
- Routine
- Hashtag

---
#### ExerciseHasType
**Properties**
- ExerciseId *int*  
- TypeId *int*

**Navigation Properties**
- Exercise
- Type

---
#### ExerciseHasModality
**Properties**
- ExerciseId *int*  
- ModalityId *int*

**Navigation Properties**
- Exercise
- Modality

---
#### ExerciseHasGoal
**Properties**
- ExerciseId *int*  
- GoalId *int*

**Navigation Properties**
- Exercise
- Goal

---
#### ExerciseHasHashtag
**Properties**
- ExerciseId *int*  
- HashtagId *int*

**Navigation Properties**
- Exercise
- Hashtag

---
#### ExerciseHasVariation
**Properties**
- ExerciseId *int*  
- VariationId *int*

**Navigation Properties**
- Exercise
- Variation

---
## Models
### Value Objects (Optional for future)
- Value Objects represent attributes that do not have identity.
- Currently not used, but examples could include `Address`, `ContactInformation`, etc., if necessary in future domain expansions.

---
## Infrastructure
### DbContext
The `AppDbContext` is the central component that maps domain entities to the database using **Entity Framework Core**.

It represents a **session with the database** and acts as the **entry point for all persistence operations**, enabling entity tracking, change management, query translation, and configuration of database relationships and constraints.

It implements the `IAppDbContext` interface, promoting **decoupling**, **testability**, and adherence to **clean architecture**.

> It is injected into all repositories and the `UnitOfWork` class.

#### Responsibilities
|Feature|Description|
|---|---|
|`DbSet<TEntity>`|Each entity is mapped to a table through a strongly-typed collection.|
|`OnModelCreating`|Configures entity relationships, keys, indexes, and constraints.|
|`DbContextOptions`|Injected via constructor, supports external configuration.|
|**Abstraction (`IAppDbContext`)**|Enables separation of concerns and unit testing.|
|**Transaction Participation**|Supports `UnitOfWork` for consistent commits and rollbacks.|
|**Auditing Support**|Can support fields like `CreatedAt`, `UpdatedAt`, and custom session variables like `@user_id`.|
|**Migrations**|Used as the base context to generate and apply database migrations.|

---
#### Interface: `IAppDbContext`
The `IAppDbContext` defines only the necessary contracts for the domain and application layers to interact with EF Core without tight coupling.

```csharp
public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Instructor> Instructors { get; }
    DbSet<Goal> Goals { get; }
    DbSet<Level> Levels { get; }
    DbSet<Type> Types { get; }
    DbSet<Modality> Modalities { get; }
    DbSet<Hashtag> Hashtags { get; }
    DbSet<Workout> Workouts { get; }
    DbSet<Routine> Routines { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<RoutineHasExercise> RoutineHasExercises { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

---
### Unit of Work
The **Unit of Work (UoW)** is a central coordination point for:
- **Managing database transactions**
- **Accessing all repositories**
- **Controlling the persistence lifecycle**
- **Propagating contextual session variables**, such as `@user_id` (used for logging/audit in MySQL)

It ensures that all operations within a request can be **executed as a single atomic unit**, with full rollback in case of failure.

#### IUnitOfWork (Domain Layer)
The `IUnitOfWork` interface is defined in the **Domain layer**, exposing:
- Access to all repositories
- Transaction control (`BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`)
- Persistence (`SaveAsync`, `SaveAndCommitAsync`)
- Utility operations for encapsulating work inside a safe transactional context


```csharp
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
    Task SaveAsync();
    Task SaveAndCommitAsync();
    Task<TResult> BeginAndCommitAsync<TResult>(int userId, Func<Task<TResult>> operation);
    Task BeginAndCommitAsync(int userId, Func<Task> operation);
    bool HasPendingChanges();
}
```

---
### IConnectionManager Interface
The `IConnectionManager` interface defines the contract for retrieving database connection strings based on user roles.

This abstraction allows the application to dynamically determine which database credentials should be used for a given user context without tying the business logic to infrastructure-specific details.

```csharp
public interface IConnectionManager
{
    string GetConnectionString(string role);
}
```

```json
{
  "ConnectionStrings": {
    "EasyTrainerAdmin": "Server=localhost;Database=easytrainer;User=easytrainer_admin;Password=adminpassword;",
    "EasyTrainerInstructor": "Server=localhost;Database=easytrainer;User=easytrainer_instructor;Password=instructorpassword;",
    "EasyTrainerUser": "Server=localhost;Database=easytrainer;User=easytrainer_user;Password=userpassword;"
  }
}
```

---
### Repositories Interfaces

#### Generic Repository
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
#### IGenericRepository

```csharp
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
#### Main Repositories Interfaces
- `IUserRepository`

```csharp
public interface IUserRepository : IGenericRepository<User>
{
	Task<User?> GetUserByEmailAsync(string email);
	Task<IEnumerable<User>> GetUsersByInstructorIdAsync(int instructorId);
	Task<IEnumerable<User>> GetUsersByGoalIdAsync(int goalId);
	Task<IEnumerable<User>> GetUsersByLevelIdAsync(int levelId);
	Task<IEnumerable<User>> GetUsersByWorkoutIdAsync(int workoutId);
	Task<bool> ExistsByEmailAsync(string email);
	Task<bool> ExistsByIdAsync(int id);
	Task<bool> IsEmailTakenByOtherUserAsync(string email, int currentUserId);
}
```

- `IInstructorRepository`

```csharp
public interface IInstructorRepository : IGenericRepository<Instructor>
{
	Task<Instructor?> GetInstructorByEmailAsync(string email);
	Task<IEnumerable<Instructor>> GetInstructorsByUserIdAsync(int userId);
	Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId);
	Task<Instructor?> GetInstructorByRoutineIdAsync(int routineId);
	Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId);
	Task<bool> ExistsByIdAsync(int id);
	Task<bool> ExistsByEmailAsync(string email);
	Task<bool> IsEmailTakenByOtherAsync(string email, int currentId);
}
```

- `IGoalRepository`

```csharp
public interface IGoalRepository : IGenericRepository<Goal>
{
	Task<IEnumerable<Goal>> GetGoalsByUserAsync(int userId);
	Task<IEnumerable<Goal>> GetGoalsByWorkoutAsync(int workoutId);
	Task<IEnumerable<Goal>> GetGoalsByRoutineAsync(int routineId);
	Task<IEnumerable<Goal>> GetGoalsByExerciseAsync(int exerciseId);
	Task<bool> ExistsByIdAsync(int id);
}
```


- `ILevelRepository`

```csharp
public interface ILevelRepository : IGenericRepository<Level>
{
	Task<Level?> GetLevelByUserAsync(int userId);
	Task<Level?> GetLevelByWorkoutAsync(int workoutId);
	Task<Level?> GetLevelByRoutineAsync(int routineId);
	Task<Level?> GetLevelByExerciseAsync(int exerciseId);
	Task<bool> ExistsByIdAsync(int id);
}
```


- `ITypeRepository`

```csharp
public interface ITypeRepository : IGenericRepository<TrainingType>
{
	Task<IEnumerable<TrainingType>> GetTypesByWorkoutAsync(int workoutId);
	Task<IEnumerable<TrainingType>> GetTypesByRoutineAsync(int routineId);
	Task<IEnumerable<TrainingType>> GetTypesByExerciseAsync(int exerciseId);
	Task<bool> ExistsByIdAsync(int id);
}
```


- `IModalityRepository`

```csharp
public interface IModalityRepository : IGenericRepository<Modality>
{
	Task<IEnumerable<Modality>> GetModalitiesByWorkoutAsync(int workoutId);
	Task<IEnumerable<Modality>> GetModalitiesByRoutineAsync(int routineId);
	Task<IEnumerable<Modality>> GetModalitiesByExerciseAsync(int exerciseId);
}
```


- `IHashtagRepository`

```csharp
public interface IHashtagRepository : IGenericRepository<Hashtag>
{
	Task<IEnumerable<Hashtag>> GetHashtagsByWorkoutAsync(int workoutId);
	Task<IEnumerable<Hashtag>> GetHashtagsByRoutineAsync(int routineId);
	Task<IEnumerable<Hashtag>> GetHashtagsByExerciseAsync(int exerciseId);
	Task<bool> ExistsByIdAsync(int id);
}
```


- `IWorkoutRepository`

```csharp
public interface IWorkoutRepository : IGenericRepository<Workout>
{
    Task<Instructor?> GetInstructorByWorkoutIdAsync(int workoutId);

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

    Task<Workout?> GetByIdAsync(int id);
    Task DeleteByIdAsync(int id);
    Task<bool> ExistsByIdAsync(int id);

    Task<IEnumerable<Workout>> GetWorkoutsByUserIdAsync(int userId);
    Task<IEnumerable<Workout>> GetWorkoutsByInstructorIdAsync(int instructorId);
    Task<IEnumerable<Workout>> GetWorkoutsByGoalIdAsync(int goalId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByLevelIdAsync(int levelId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByTypeIdAsync(int typeId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByModalityIdAsync(int modalityId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByHashtagIdAsync(int hashtagId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByRoutineIdAsync(int routineId, int? instructorId = null, int? userId = null);
    Task<IEnumerable<Workout>> GetWorkoutsByExerciseIdAsync(int exerciseId, int? instructorId = null, int? userId = null);
}
```


- `IRoutineRepository`

```csharp
public interface IRoutineRepository : IGenericRepository<Routine>
{
	Task<IEnumerable<Routine>> GetRoutinesByInstructorIdAsync(int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByGoalIdAsync(int goalId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByLevelIdAsync(int levelId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByTypeIdAsync(int typeId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByModalityAsync(int modalityId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByHashtagAsync(int hashtagId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByWorkoutAsync(int workoutId, int instructorId);
	Task<IEnumerable<Routine>> GetRoutinesByExerciseAsync(int exerciseId, int instructorId);
}
```


- `IExerciseRepository`

```csharp
public interface IExerciseRepository : IGenericRepository<Exercise>
{
	Task<IEnumerable<Exercise>> GetExercisesByInstructorIdAsync(int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByGoalIdAsync(int goalId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByLevelIdAsync(int levelId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByTypeIdAsync(int typeId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByModalityIdAsync(int modalityId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByHashtagIdAsync(int hashtagId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByRoutineIdAsync(int routineId, int instructorId);
	Task<IEnumerable<Exercise>> GetExercisesByWorkoutIdAsync(int workoutId, int instructorId);
	Task<IEnumerable<Exercise>> GetVariationsByExerciseAsync(int exerciseId, int instructorId);

	Task<Exercise?> GetByIdAsync(int id);
	Task DeleteByIdAsync(int id);
	Task<bool> ExistsByIdAsync(int id);

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

	Task<Instructor?> GetInstructorByExerciseIdAsync(int exerciseId);
}
```

---
#### Relationship Repositories Interfaces
For relationships containing additional attributes, we define specific repositories.
- `IRoutineHasExerciseRepository`

```csharp
public interface IRoutineHasExerciseRepository
{
	Task AddAsync(RoutineHasExercise entity);
	Task<RoutineHasExercise?> GetByIdAsync(int routineId, int exerciseId);
	Task UpdateAsync(RoutineHasExercise entity);
	Task DeleteByIdAsync(int routineId, int exerciseId);

	Task<IEnumerable<RoutineHasExercise>> GetExercisesByRoutineIdAsync(int routineId);
	Task<IEnumerable<RoutineHasExercise>> GetRoutinesByExerciseIdAsync(int exerciseId);
	Task SaveAsync();
}
```

---
#### Notes
- Only `RoutineHasExercise` has a dedicated repository because it contains additional business attributes (order, sets, reps, rest time, notes, day, week, is_optional).
- All other relationship tables (simple many-to-many links) are handled through entity navigation and do not require separate repositories.
