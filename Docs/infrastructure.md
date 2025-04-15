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
## Repositories
Repositories in Infrastructure **implement the repository interfaces** defined in the Domain layer.  
They encapsulate:
- **Standard CRUD operations**.    
- **Advanced querying and filtering** when needed.
- **Transaction support** through the Unit of Work pattern.

### Generic Repository
The **GenericRepository** provides a reusable implementation of common database operations.

```
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
## Connection Manager
The `ConnectionManager` is responsible for **providing the correct database connection string** based on the **authenticated user's role**.

It offers:
- **Security**: Minimal privilege principle by separating access by roles.
- **Flexibility**: Supports easy expansion to multi-tenancy.
- **Centralization**: All connection logic is controlled in a single place.

```
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

```
{
  "ConnectionStrings": {
    "EasyTrainerAdmin": "Server=localhost;Database=easytrainer;User=easytrainer_admin;Password=adminpassword;",
    "EasyTrainerInstructor": "Server=localhost;Database=easytrainer;User=easytrainer_instructor;Password=instructorpassword;",
    "EasyTrainerUser": "Server=localhost;Database=easytrainer;User=easytrainer_user;Password=userpassword;"
  }
}
```
