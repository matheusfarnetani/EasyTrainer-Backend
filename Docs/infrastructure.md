# Infrastructure
## Repositories
- Implement interfaces defined in the Domain layer. 
- Use EF Core for database operations.
- Transactions will be managed by Unit of Work.

### Generic Repository
```
public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BrandDbContext _databaseContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BrandDbContext context)
        {
            _databaseContext = context;
            _dbSet = context.Set<T>();
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

        public async Task<List<T>> GetAllAsync(bool tracked = true)
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
- UserRepository
- InstructorRepository
- GoalRepository
- LevelRepository
- TypeRepository
- ModalityRepository
- HashtagRepository
- WorkoutRepository
- RoutineRepository
- ExerciseRepository

---
### Relationship Repositories
- RoutineHasExerciseRepository

---
## ConnectionManager Class
The `ConnectionManager` class implements the `IConnectionManager` interface and provides the actual logic for retrieving database connection strings from the application's configuration.

It encapsulates the selection logic based on the authenticated user's role and ensures that users connect to the database with the appropriate privileges.

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

```
{
  "ConnectionStrings": {
    "EasyTrainerAdmin": "Server=localhost;Database=easytrainer;User=easytrainer_admin;Password=adminpassword;",
    "EasyTrainerInstructor": "Server=localhost;Database=easytrainer;User=easytrainer_instructor;Password=instructorpassword;",
    "EasyTrainerUser": "Server=localhost;Database=easytrainer;User=easytrainer_user;Password=userpassword;"
  }
}
```
