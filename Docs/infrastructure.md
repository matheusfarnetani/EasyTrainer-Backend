# Infrastructure
The **Infrastructure layer** provides concrete implementations of abstractions defined in the **Domain layer**.

Its main responsibilities are:
- **Persistence**: Managing database operations using **Entity Framework Core**.
- **Transaction Management**: Coordinating commits and rollbacks through a centralized **Unit of Work**.
- **External Resources**: Providing access to external systems like storage services, authentication providers, and third-party APIs (future).

The Infrastructure layer is purely technical. It **depends** on:
- The **Domain layer** for interfaces and abstractions.
- **Configuration settings** (e.g., connection strings).
- **External libraries** (e.g., EF Core, Redis, JWT providers).

It must **never contain business logic**.

---
### DbContext
The `AppDbContext` is the central component that maps domain entities to the database using **Entity Framework Core**.

It represents a **session with the database** and acts as the **entry point for all persistence operations**, enabling entity tracking, change management, query translation, and configuration of database relationships and constraints.

It implements the `IAppDbContext` interface, promoting **decoupling**, **testability**, and adherence to **clean architecture**.

> It is injected into all repositories and the `UnitOfWork` class.

---
#### Responsibilities

|Feature|Description|
|---|---|
|`DbSet<TEntity>`|Each entity is mapped to a table through a strongly-typed collection.|
|`OnModelCreating`|Automatically loads all configuration classes that implement `IEntityTypeConfiguration<T>`.|
|`DbContextOptions`|Injected via constructor, supports external configuration.|
|**Abstraction (`IAppDbContext`)**|Enables separation of concerns and unit testing.|
|**Transaction Participation**|Supports `UnitOfWork` for consistent commits and rollbacks.|
|**Auditing Support**|Can support fields like `CreatedAt`, `UpdatedAt`, and custom session variables like `@user_id`.|
|**Migrations**|Used as the base context to generate and apply database migrations.|

---
#### Implementation: `AppDbContext.cs`

```csharp
public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<Type> Types => Set<Type>();
    public DbSet<Modality> Modalities => Set<Modality>();
    public DbSet<Hashtag> Hashtags => Set<Hashtag>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Routine> Routines => Set<Routine>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<RoutineHasExercise> RoutineHasExercises => Set<RoutineHasExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically apply all IEntityTypeConfiguration<TEntity> classes from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
```

---
#### Registration in Program.cs

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var role = GetCurrentUserRole(); // Resolved via ConnectionManager or token
    var connectionString = connectionManager.GetConnectionString(role);
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
```

---
## Entity Configurations
In order to follow **separation of concerns** and improve maintainability, all entity mapping logic should be moved from `OnModelCreating()` to dedicated **configuration classes** using the `IEntityTypeConfiguration<TEntity>` interface.

### Purpose
This approach allows each entity's mapping to be:
- **Defined in isolation**
- **Easily testable**
- **Reused** or extended without modifying the DbContext
- **Simplified in structure**, keeping `AppDbContext.cs` clean and focused

---
### Responsibilities of Entity Configurations
|Feature|Description|
|---|---|
|Define Primary and Composite Keys|Specify `HasKey`, `HasAlternateKey`|
|Configure Relationships|`HasOne`, `WithMany`, `HasForeignKey`, `OnDelete`, etc.|
|Configure Indexes|`HasIndex().IsUnique()`|
|Define Column Constraints|`HasMaxLength`, `IsRequired`, `HasDefaultValue`, etc.|
|Table/Schema Naming|`ToTable("name")`, `ToSchema("schema")`|

### Configurations
- `UserConfiguration.cs`

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Password).IsRequired();

        builder.HasOne(u => u.Level)
               .WithMany()
               .HasForeignKey(u => u.LevelId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Instructor)
               .WithMany()
               .HasForeignKey(u => u.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
```

- `InstructorConfiguration.cs`

```csharp
public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("instructors");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
        builder.Property(i => i.Email).IsRequired().HasMaxLength(255);
        builder.Property(i => i.Password).IsRequired();

        builder.HasIndex(i => i.Email).IsUnique();
    }
}
```

- `GoalConfiguration`

```csharp
public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("goals");

        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Description).HasMaxLength(300);
    }
}
```

- `LevelConfiguration`

```csharp
public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.ToTable("levels");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
        builder.Property(l => l.Description).HasMaxLength(300);
    }
}
```

- `TypeConfiguration`

```csharp
public class TypeConfiguration : IEntityTypeConfiguration<Type>
{
    public void Configure(EntityTypeBuilder<Type> builder)
    {
        builder.ToTable("types");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(300);
    }
}
```

- `ModalityConfiguration`

```csharp
public class ModalityConfiguration : IEntityTypeConfiguration<Modality>
{
    public void Configure(EntityTypeBuilder<Modality> builder)
    {
        builder.ToTable("modalities");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        builder.Property(m => m.Description).HasMaxLength(300);
    }
}
```

- `HashtagConfiguration`

```csharp
public class HashtagConfiguration : IEntityTypeConfiguration<Hashtag>
{
    public void Configure(EntityTypeBuilder<Hashtag> builder)
    {
        builder.ToTable("hashtags");

        builder.HasKey(h => h.Id);
        builder.Property(h => h.Tag).IsRequired().HasMaxLength(100);
    }
}
```

- `WorkoutConfiguration`

```csharp
public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workouts");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name).IsRequired().HasMaxLength(100);
        builder.Property(w => w.Description).HasMaxLength(300);
        builder.Property(w => w.ImageUrl).HasMaxLength(300);
        builder.Property(w => w.Duration).IsRequired();
        builder.Property(w => w.NumberOfDays).IsRequired();
        builder.Property(w => w.Indoor).IsRequired();

        builder.HasOne(w => w.Instructor)
               .WithMany()
               .HasForeignKey(w => w.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.Level)
               .WithMany()
               .HasForeignKey(w => w.LevelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
```

- `RoutineConfiguration`

```csharp
public class RoutineConfiguration : IEntityTypeConfiguration<Routine>
{
    public void Configure(EntityTypeBuilder<Routine> builder)
    {
        builder.ToTable("routines");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Description).HasMaxLength(300);
        builder.Property(r => r.ImageUrl).HasMaxLength(300);
        builder.Property(r => r.Duration).IsRequired();

        builder.HasOne(r => r.Instructor)
               .WithMany()
               .HasForeignKey(r => r.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Level)
               .WithMany()
               .HasForeignKey(r => r.LevelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
```

- `ExerciseConfiguration`

```csharp
public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(300);
        builder.Property(e => e.Equipment).HasMaxLength(100);
        builder.Property(e => e.Duration).IsRequired();
        builder.Property(e => e.Repetition).IsRequired();
        builder.Property(e => e.Sets).IsRequired();
        builder.Property(e => e.RestTime).IsRequired();
        builder.Property(e => e.BodyPart).HasMaxLength(100);
        builder.Property(e => e.VideoUrl).HasMaxLength(300);
        builder.Property(e => e.ImageUrl).HasMaxLength(300);
        builder.Property(e => e.Steps).HasMaxLength(1000);
        builder.Property(e => e.Contraindications).HasMaxLength(500);
        builder.Property(e => e.SafetyTips).HasMaxLength(500);
        builder.Property(e => e.CommonMistakes).HasMaxLength(500);
        builder.Property(e => e.IndicatedFor).HasMaxLength(500);
        builder.Property(e => e.CaloriesBurnedEstimate).HasColumnType("float");

        builder.HasOne(e => e.Instructor)
               .WithMany()
               .HasForeignKey(e => e.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Level)
               .WithMany()
               .HasForeignKey(e => e.LevelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
```

- `UserHasGoalConfiguration`

```csharp
public class UserHasGoalConfiguration : IEntityTypeConfiguration<UserHasGoal>
{
    public void Configure(EntityTypeBuilder<UserHasGoal> builder)
    {
        builder.ToTable("user_has_goal");

        builder.HasKey(x => new { x.UserId, x.GoalId });

        builder.HasOne(x => x.User)
               .WithMany(u => u.UserGoals)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Goal)
               .WithMany(g => g.UserGoals)
               .HasForeignKey(x => x.GoalId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `UserHasInstructorConfiguration`

```csharp
public class UserHasInstructorConfiguration : IEntityTypeConfiguration<UserHasInstructor>
{
    public void Configure(EntityTypeBuilder<UserHasInstructor> builder)
    {
        builder.ToTable("user_has_instructor");

        builder.HasKey(x => new { x.UserId, x.InstructorId });

        builder.HasOne(x => x.User)
               .WithMany(u => u.UserInstructors)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Instructor)
               .WithMany(i => i.UserInstructors)
               .HasForeignKey(x => x.InstructorId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasTypeConfiguration`

```csharp
public class WorkoutHasTypeConfiguration : IEntityTypeConfiguration<WorkoutHasType>
{
    public void Configure(EntityTypeBuilder<WorkoutHasType> builder)
    {
        builder.ToTable("workout_has_type");

        builder.HasKey(x => new { x.WorkoutId, x.TypeId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutTypes)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Type)
               .WithMany(t => t.WorkoutTypes)
               .HasForeignKey(x => x.TypeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasModalityConfiguration`

```csharp
public class WorkoutHasModalityConfiguration : IEntityTypeConfiguration<WorkoutHasModality>
{
    public void Configure(EntityTypeBuilder<WorkoutHasModality> builder)
    {
        builder.ToTable("workout_has_modality");

        builder.HasKey(x => new { x.WorkoutId, x.ModalityId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutModalities)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Modality)
               .WithMany(m => m.WorkoutModalities)
               .HasForeignKey(x => x.ModalityId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasGoalConfiguration`

```csharp
public class WorkoutHasGoalConfiguration : IEntityTypeConfiguration<WorkoutHasGoal>
{
    public void Configure(EntityTypeBuilder<WorkoutHasGoal> builder)
    {
        builder.ToTable("workout_has_goal");

        builder.HasKey(x => new { x.WorkoutId, x.GoalId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutGoals)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Goal)
               .WithMany(g => g.WorkoutGoals)
               .HasForeignKey(x => x.GoalId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasHashtagConfiguration`

```csharp
public class WorkoutHasHashtagConfiguration : IEntityTypeConfiguration<WorkoutHasHashtag>
{
    public void Configure(EntityTypeBuilder<WorkoutHasHashtag> builder)
    {
        builder.ToTable("workout_has_hashtag");

        builder.HasKey(x => new { x.WorkoutId, x.HashtagId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutHashtags)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Hashtag)
               .WithMany(h => h.WorkoutHashtags)
               .HasForeignKey(x => x.HashtagId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasUserConfiguration`

```csharp
public class WorkoutHasUserConfiguration : IEntityTypeConfiguration<WorkoutHasUser>
{
    public void Configure(EntityTypeBuilder<WorkoutHasUser> builder)
    {
        builder.ToTable("workout_has_user");

        builder.HasKey(x => new { x.WorkoutId, x.UserId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutUsers)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
               .WithMany(u => u.WorkoutUsers)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasRoutineConfiguration`

```csharp
public class WorkoutHasRoutineConfiguration : IEntityTypeConfiguration<WorkoutHasRoutine>
{
    public void Configure(EntityTypeBuilder<WorkoutHasRoutine> builder)
    {
        builder.ToTable("workout_has_routine");

        builder.HasKey(x => new { x.WorkoutId, x.RoutineId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutRoutines)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.WorkoutRoutines)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `WorkoutHasExerciseConfiguration`

```csharp
public class WorkoutHasExerciseConfiguration : IEntityTypeConfiguration<WorkoutHasExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutHasExercise> builder)
    {
        builder.ToTable("workout_has_exercise");

        builder.HasKey(x => new { x.WorkoutId, x.ExerciseId });

        builder.HasOne(x => x.Workout)
               .WithMany(w => w.WorkoutExercises)
               .HasForeignKey(x => x.WorkoutId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.WorkoutExercises)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `RoutineHasTypeConfiguration`

```csharp
public class RoutineHasTypeConfiguration : IEntityTypeConfiguration<RoutineHasType>
{
    public void Configure(EntityTypeBuilder<RoutineHasType> builder)
    {
        builder.ToTable("routine_has_type");

        builder.HasKey(x => new { x.RoutineId, x.TypeId });

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.RoutineTypes)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Type)
               .WithMany(t => t.RoutineTypes)
               .HasForeignKey(x => x.TypeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `RoutineHasModalityConfiguration`

```csharp
public class RoutineHasModalityConfiguration : IEntityTypeConfiguration<RoutineHasModality>
{
    public void Configure(EntityTypeBuilder<RoutineHasModality> builder)
    {
        builder.ToTable("routine_has_modality");

        builder.HasKey(x => new { x.RoutineId, x.ModalityId });

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.RoutineModalities)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Modality)
               .WithMany(m => m.RoutineModalities)
               .HasForeignKey(x => x.ModalityId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `RoutineHasGoalConfiguration`

```csharp
public class RoutineHasGoalConfiguration : IEntityTypeConfiguration<RoutineHasGoal>
{
    public void Configure(EntityTypeBuilder<RoutineHasGoal> builder)
    {
        builder.ToTable("routine_has_goal");

        builder.HasKey(x => new { x.RoutineId, x.GoalId });

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.RoutineGoals)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Goal)
               .WithMany(g => g.RoutineGoals)
               .HasForeignKey(x => x.GoalId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `RoutineHasExerciseConfiguration`

```csharp
public class RoutineHasExerciseConfiguration : IEntityTypeConfiguration<RoutineHasExercise>
{
    public void Configure(EntityTypeBuilder<RoutineHasExercise> builder)
    {
        builder.ToTable("routine_has_exercise");

        builder.HasKey(x => new { x.RoutineId, x.ExerciseId });

        builder.Property(x => x.Order).IsRequired();
        builder.Property(x => x.Sets).IsRequired();
        builder.Property(x => x.Reps).IsRequired();
        builder.Property(x => x.RestTime).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(300);
        builder.Property(x => x.Day).IsRequired();
        builder.Property(x => x.Week).IsRequired();
        builder.Property(x => x.IsOptional).IsRequired();

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.RoutineExercises)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.RoutineExercises)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `RoutineHasHashtagConfiguration`

```csharp
public class RoutineHasHashtagConfiguration : IEntityTypeConfiguration<RoutineHasHashtag>
{
    public void Configure(EntityTypeBuilder<RoutineHasHashtag> builder)
    {
        builder.ToTable("routine_has_hashtag");

        builder.HasKey(x => new { x.RoutineId, x.HashtagId });

        builder.HasOne(x => x.Routine)
               .WithMany(r => r.RoutineHashtags)
               .HasForeignKey(x => x.RoutineId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Hashtag)
               .WithMany(h => h.RoutineHashtags)
               .HasForeignKey(x => x.HashtagId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `ExerciseHasTypeConfiguration`

```csharp
public class ExerciseHasTypeConfiguration : IEntityTypeConfiguration<ExerciseHasType>
{
    public void Configure(EntityTypeBuilder<ExerciseHasType> builder)
    {
        builder.ToTable("exercise_has_type");

        builder.HasKey(x => new { x.ExerciseId, x.TypeId });

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.ExerciseTypes)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Type)
               .WithMany(t => t.ExerciseTypes)
               .HasForeignKey(x => x.TypeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `ExerciseHasModalityConfiguration`

```csharp
public class ExerciseHasModalityConfiguration : IEntityTypeConfiguration<ExerciseHasModality>
{
    public void Configure(EntityTypeBuilder<ExerciseHasModality> builder)
    {
        builder.ToTable("exercise_has_modality");

        builder.HasKey(x => new { x.ExerciseId, x.ModalityId });

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.ExerciseModalities)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Modality)
               .WithMany(m => m.ExerciseModalities)
               .HasForeignKey(x => x.ModalityId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `ExerciseHasGoalConfiguration`

```csharp
public class ExerciseHasGoalConfiguration : IEntityTypeConfiguration<ExerciseHasGoal>
{
    public void Configure(EntityTypeBuilder<ExerciseHasGoal> builder)
    {
        builder.ToTable("exercise_has_goal");

        builder.HasKey(x => new { x.ExerciseId, x.GoalId });

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.ExerciseGoals)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Goal)
               .WithMany(g => g.ExerciseGoals)
               .HasForeignKey(x => x.GoalId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `ExerciseHasHashtagConfiguration`

```csharp
public class ExerciseHasHashtagConfiguration : IEntityTypeConfiguration<ExerciseHasHashtag>
{
    public void Configure(EntityTypeBuilder<ExerciseHasHashtag> builder)
    {
        builder.ToTable("exercise_has_hashtag");

        builder.HasKey(x => new { x.ExerciseId, x.HashtagId });

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.ExerciseHashtags)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Hashtag)
               .WithMany(h => h.ExerciseHashtags)
               .HasForeignKey(x => x.HashtagId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

- `ExerciseHasVariationConfiguration`

```csharp
public class ExerciseHasVariationConfiguration : IEntityTypeConfiguration<ExerciseHasVariation>
{
    public void Configure(EntityTypeBuilder<ExerciseHasVariation> builder)
    {
        builder.ToTable("exercise_has_variation");

        builder.HasKey(x => new { x.ExerciseId, x.VariationId });

        builder.HasOne(x => x.Exercise)
               .WithMany(e => e.ExerciseVariations)
               .HasForeignKey(x => x.ExerciseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Variation)
               .WithMany(e => e.IsVariationOf)
               .HasForeignKey(x => x.VariationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

---
## Unit of Work
The **Unit of Work (UoW)** is a central coordination point for:
- **Managing database transactions**
- **Accessing all repositories**
- **Controlling the persistence lifecycle**
- **Propagating contextual session variables**, such as `@user_id` (used for logging/audit in MySQL)

It ensures that all operations within a request can be **executed as a single atomic unit**, with full rollback in case of failure.

### UnitOfWork (Infrastructure Layer)
The concrete implementation of `IUnitOfWork` is defined in the **Infrastructure layer**.  
It uses EF Core's `DbContext` and `IDbContextTransaction` to ensure safe, coordinated operations.

Key responsibilities:
- Begin transaction and set `@user_id` in MySQL session
- Commit or rollback changes
- Automatically dispose transactions
- Wrap arbitrary logic with `BeginAndCommitAsync` helpers
- Log commit errors safely using `ILogger`

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

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

    // Methods...
}
```

|Feature|Benefit|
|---|---|
|`@user_id` session variable|Enables backend-side tracking of **who made the change** without extra parameters|
|`BeginAndCommitAsync()`|Allows wrapping service logic in a safe, reusable, transactional block|
|Centralized Save & Rollback|Keeps logic **clean** and **robust**|
|Easy DI setup|Exposes all repositories in a single point for service injection|
|Logging support|Captures commit errors for better observability|

---
## Connection Manager
The `ConnectionManager` is responsible for **providing the correct database connection string** based on the **authenticated user's role**.

It offers:
- **Security**: Minimal privilege principle by separating access by roles.
- **Flexibility**: Supports easy expansion to multi-tenancy.
- **Centralization**: All connection logic is controlled in a single place.

```csharp
using Microsoft.Extensions.Configuration;

public class ConnectionManager : IConnectionManager
{
    private readonly IConfiguration _configuration;

    public ConnectionManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString(string role)
    {
        return role.ToLower() switch
        {
            "admin" => _configuration.GetConnectionString("EasyTrainerAdmin"),
            "instructor" => _configuration.GetConnectionString("EasyTrainerInstructor"),
            "user" => _configuration.GetConnectionString("EasyTrainerUser"),
            _ => throw new ArgumentException("Invalid user role provided for connection.")
        };
    }
}
```

### Example appsettings.json

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
## Repositories
Repositories in Infrastructure **implement the repository interfaces** defined in the Domain layer.  
They encapsulate:
- **Standard CRUD operations**.    
- **Advanced querying and filtering** when needed.
- **Transaction support** through the Unit of Work pattern.

### Generic Repository
The **GenericRepository** provides a reusable implementation of common database operations.

```csharp
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _databaseContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = databaseContext.Set<T>();
    }
    
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
            await SaveAsync();
        }
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool tracked = true)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        return await query.ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }
}
```

---
### Main Repositories
Each **Main Model** has a corresponding repository that inherits from `GenericRepository<T>`  
and can optionally add **custom query methods** when needed.

|Repository|Responsibility|
|---|---|
|`UserRepository`|Manage users and user-related queries (goals, workouts, instructors)|
|`InstructorRepository`|Manage instructors and their created content|
|`GoalRepository`|Manage fitness goals|
|`LevelRepository`|Manage difficulty levels|
|`TypeRepository`|Manage exercise and workout types|
|`ModalityRepository`|Manage training modalities|
|`HashtagRepository`|Manage hashtags for exercises/workouts|
|`WorkoutRepository`|Manage workouts and related relationships|
|`RoutineRepository`|Manage workout routines and exercises inside|
|`ExerciseRepository`|Manage exercises and their variations|

---
### Relationship Repositories
Some relationship tables **contain additional business attributes** (e.g., `RoutineHasExercise` includes `order`, `sets`, `reps`, `restTime`, etc.).  
These require dedicated repositories.

|Repository|Responsibility|
|---|---|
|`RoutineHasExerciseRepository`|Manage exercises inside routines, respecting order, sets, and repetitions.|

> **Simple many-to-many relationships** (without extra attributes) are handled natively by EF Core through navigation properties.

---
## Health Checks
The **Health Check** mechanism ensures that the application can successfully connect to the database and that the underlying infrastructure is responsive.

It improves system **reliability**, **CI/CD confidence**, and enables **external monitoring tools** (like Kubernetes, Azure, AWS, Docker) to probe the API's health.

### Purpose
- Validate database availability at runtime
- Enable automated uptime checks
- Detect and respond to failures proactively
- Support readiness and liveness probes in production environments

---
### Responsibilities
|Feature|Description|
|---|---|
|`AddDbContextCheck<T>()`|Verifies whether EF Core can connect to the database|
|`MapHealthChecks()`|Exposes a simple endpoint (e.g., `/health`) for health probes|
|CI/CD Integration|Helps fail fast if the database is unreachable during deploy|
|Production Monitoring|Enables load balancers or orchestrators to detect failures|

---
### Example Setup
In `Program.cs`:

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("MySQL Database");

app.MapHealthChecks("/health");
```

Once registered, hitting `/health` will return:
- `200 OK` when the DB is reachable
- `503 Service Unavailable` if not

> You can access `/health` directly or integrate it with monitoring dashboards, deployment scripts, and orchestration tools.
