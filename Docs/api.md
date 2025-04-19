# API
## Introduction
The **EasyTrainer API** is responsible for handling communication between external clients (web, mobile, admin panel) and the internal application layer.  
It is designed following modern backend architecture principles:
- **RESTful design** with clean and semantic HTTP methods.
- **API Versioning** (`/api/v1/`) for safe future evolutions.
- **Layered architecture**, separating concerns across Middlewares, Controllers, Services.
- **Secure communication** (JWT Authentication).
- **Consistent API responses** (enveloping all outputs).
- **Scalable and Modular**: easy to extend, maintain, and document.

---
## API Structure
| Component                | Description                                                                      |
| ------------------------ | -------------------------------------------------------------------------------- |
| **Routes**               | Define the available HTTP endpoints following REST conventions                   |
| **Controllers**          | Thin controllers responsible only for input validation and service orchestration |
| **Middlewares**          | Cross-cutting layers for error handling, authentication, logging                 |
| **Dependency Injection** | All components are wired via the native .NET DI container                        |
| **Response Envelopes**   | All responses are wrapped in standardized formats (`ApiResponseDTO<T>`)          |
| **Pagination**           | Pagination supported in all list endpoints                                       |
| **Authentication**       | JWT authentication applied where necessary                                       |
| **Caching/Future**       | Ready for adding Caching, Rate-Limiting, API Gateway                             |

---
## Middlewares
Middlewares are **cross-cutting components** in the API pipeline that handle global concerns independently from the business logic.
They are executed in the order they are registered in the pipeline.

|Middleware|Purpose|
|---|---|
|**Global Error Handling Middleware**|Capture unhandled exceptions and return consistent API error responses|
|**JWT Authentication Middleware**|Protect sensitive endpoints by validating JWT tokens|
|**Request/Response Logging Middleware** (optional)|Log HTTP traffic for debugging and auditing purposes|

### Global Error Handling Middleware
**Purpose**:  
Catch all unhandled exceptions during request processing and return a structured `ApiResponseDTO` instead of a raw error.

**Responsibilities**:
- Catch all runtime exceptions.
- Return an HTTP status code (e.g., 500 Internal Server Error) when unexpected errors occur.
- Wrap the error message inside a `ApiResponseDTO` with `Success=false`.    
- Optionally log the exception details for internal debugging.

**Benefits**:
- Prevents leaking stack traces or internal errors to clients.
- Makes frontend integration easier with predictable error formats.
- Centralizes error management.

---
### JWT Authentication Middleware
**Purpose**:  
Authenticate and authorize incoming requests using JWT tokens.

**Responsibilities**:
- Validate the JWT signature and expiration.
- Extract claims from the token (e.g., User ID, Role).
- Attach the authenticated user information into the HTTP Context for later use in controllers or services.

**Benefits**:
- Protects sensitive endpoints.
- Enables role-based or user-based authorization.
- Facilitates user tracking for audit trails.

---
### Request/Response Logging Middleware
**Purpose**:  
Log critical request and response data for auditing, debugging, and monitoring.

**Responsibilities**:
- Log incoming requests (URL, method, headers, optionally body).
- Log outgoing responses (status code, headers, optionally body).
- Avoid logging sensitive fields (e.g., passwords).

**Benefits**:
- Helps diagnose production issues.
- Provides a clear audit trail for security.
- Enables performance analysis and monitoring.

---
## Controllers
### Controller Philosophy
Controllers must remain **thin**:
- **Do not implement business rules**.
- **Do not access the database directly**.
- **Do not leak internal exceptions**.
- **Only call Services** and return formatted responses.

> "**Controllers exist to receive HTTP requests, validate them, delegate processing to services, and return a standardized response.**"

### Controller Model (Template)

```csharp
[ApiController]
[Route("easytrainer/api/v1/[controller]")]
public class EntityNameController : ControllerBase
{
    private readonly IEntityNameService _entityNameService;

    public EntityNameController(IEntityNameService entityNameService)
    {
        _entityNameService = entityNameService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _entityNameService.GetByIdAsync(id);
        return Ok(ApiResponseDTO.CreateSuccess(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEntityInputDTO dto)
    {
        var result = await _entityNameService.CreateAsync(dto);
        return Ok(ApiResponseDTO.CreateSuccess(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEntityInputDTO dto)
    {
        var result = await _entityNameService.UpdateAsync(id, dto);
        return Ok(ApiResponseDTO.CreateSuccess(result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _entityNameService.DeleteAsync(id);
        return Ok(ApiResponseDTO.CreateSuccess());
    }
}
```


---
### Controller Layer Structure
|Controller|Responsibility|
|---|---|
|UserController|Manage users and their relations (goals, instructors, workouts)|
|InstructorController|Manage instructors and their related users, workouts, routines, exercises|
|GoalController|Manage fitness goals|
|LevelController|Manage difficulty levels|
|TypeController|Manage exercise and workout types|
|ModalityController|Manage training modalities|
|HashtagController|Manage hashtags for categorization|
|WorkoutController|Manage workout programs|
|RoutineController|Manage workout routines|
|ExerciseController|Manage exercises and their variations|

---
# Routes
## Routes Introduction
The **Routes** define the HTTP interface of EasyTrainer.  
They are designed to be:
- Fully **RESTful** (GET, POST, PUT, DELETE)
- **Versioned** (`/v1/`) for future-proofing
- **Paginated** for large datasets
- **Consistent** in resource and relationship naming
- **Filterable** (especially for ownership scenarios with instructors)

All responses are wrapped using a **standard API response envelope** (`ApiResponseDTO<T>`) for consistency and error handling.

---
## Base URL

```
/easytrainer/api/v1/
```

---
## General Routing Rules
|Rule|Description|
|---|---|
|**HTTP Verbs**|GET = retrieve, POST = create, PUT = update, DELETE = remove|
|**Pagination**|Supported on list endpoints via `pageNumber` and `pageSize`|
|**Instructor Ownership**|Some queries require `instructorId` for filtering results|
|**Relationship Nesting**|Clear child routes (e.g., `/user/{id}/goals`)|
|**Standardized Responses**|All responses formatted consistently for frontend consumption|

---
## Routes
### User
**Description**: Manage users and their relationships with goals, instructors, and workouts.

|Method|Route|Description|
|---|---|---|
|GET|/user/{id}|Get user by ID|
|POST|/user|Create a new user|
|PUT|/user/{id}|Update a user|
|DELETE|/user/{id}|Delete a user|
|GET|/user|List users (with pagination)|
|GET|/user/{id}/goals|List goals linked to user|
|POST|/user/{id}/goals/{goalId}|Add a goal to user|
|DELETE|/user/{id}/goals/{goalId}|Remove a goal from user|
|GET|/user/{id}/instructors|List instructors assigned to user|
|POST|/user/{id}/instructors/{instructorId}|Assign an instructor to user|
|DELETE|/user/{id}/instructors/{instructorId}|Unassign instructor from user|
|GET|/user/{id}/workouts|List workouts assigned to user|
|POST|/user/{id}/workouts/{workoutId}|Add a workout to user|
|DELETE|/user/{id}/workouts/{workoutId}|Remove a workout from user|

---
### Instructor
**Description**: Manage instructors and list their created workouts, routines, and exercises.

|Method|Route|Description|
|---|---|---|
|GET|/instructor/{id}|Get instructor by ID|
|POST|/instructor|Create a new instructor|
|PUT|/instructor/{id}|Update instructor|
|DELETE|/instructor/{id}|Delete instructor|
|GET|/instructor|List instructors (pagination)|
|GET|/instructor/{id}/users|List users assigned to instructor|
|GET|/instructor/{id}/workouts|List workouts created by instructor|
|GET|/instructor/{id}/routines|List routines created by instructor|
|GET|/instructor/{id}/exercises|List exercises created by instructor|

---
### Goal
**Description**: Manage fitness goals and related entities.

|Method|Route|Description|
|---|---|---|
|GET|/goal/{id}|Get goal by ID|
|POST|/goal|Create goal|
|PUT|/goal/{id}|Update goal|
|DELETE|/goal/{id}|Delete goal|
|GET|/goal|List all goals (pagination)|
|GET|/goal/{id}/workouts?instructorId={instructorId}|List workouts linked to goal|
|GET|/goal/{id}/routines?instructorId={instructorId}|List routines linked to goal|
|GET|/goal/{id}/exercises?instructorId={instructorId}|List exercises linked to goal|

---
### Level
**Description**: Manage levels of difficulty and related users, workouts, routines, and exercises.

|Method|Route|Description|
|---|---|---|
|GET|/level/{id}|Get level by ID|
|POST|/level|Create level|
|PUT|/level/{id}|Update level|
|DELETE|/level/{id}|Delete level|
|GET|/level|List all levels (pagination)|
|GET|/level/{id}/users|List users with this level|
|GET|/level/{id}/workouts?instructorId={instructorId}|List workouts with this level|
|GET|/level/{id}/routines?instructorId={instructorId}|List routines with this level|
|GET|/level/{id}/exercises?instructorId={instructorId}|List exercises with this level|

---
### Type
**Description**: Manage exercise/workout types.

|Method|Route|Description|
|---|---|---|
|GET|/type/{id}|Get type by ID|
|POST|/type|Create type|
|PUT|/type/{id}|Update type|
|DELETE|/type/{id}|Delete type|
|GET|/type|List all types (pagination)|
|GET|/type/{id}/workouts?instructorId={instructorId}|List workouts by type|
|GET|/type/{id}/routines?instructorId={instructorId}|List routines by type|
|GET|/type/{id}/exercises?instructorId={instructorId}|List exercises by type|

---
### Modality
**Description**: Manage training modalities.

|Method|Route|Description|
|---|---|---|
|GET|/modality/{id}|Get modality by ID|
|POST|/modality|Create modality|
|PUT|/modality/{id}|Update modality|
|DELETE|/modality/{id}|Delete modality|
|GET|/modality|List all modalities (pagination)|
|GET|/modality/{id}/workouts?instructorId={instructorId}|List workouts by modality|
|GET|/modality/{id}/routines?instructorId={instructorId}|List routines by modality|
|GET|/modality/{id}/exercises?instructorId={instructorId}|List exercises by modality|

---
### Hashtag
**Description**: Manage hashtags for categorizing content.

|Method|Route|Description|
|---|---|---|
|GET|/hashtag/{id}|Get hashtag by ID|
|POST|/hashtag|Create hashtag|
|PUT|/hashtag/{id}|Update hashtag|
|DELETE|/hashtag/{id}|Delete hashtag|
|GET|/hashtag|List all hashtags (pagination)|
|GET|/hashtag/{id}/workouts?instructorId={instructorId}|List workouts by hashtag|
|GET|/hashtag/{id}/routines?instructorId={instructorId}|List routines by hashtag|
|GET|/hashtag/{id}/exercises?instructorId={instructorId}|List exercises by hashtag|

---
### Workout
**Description**: Manage workouts and their relationships.

|Method|Route|Description|
|---|---|---|
|GET|/workout/{id}|Get workout by ID|
|POST|/workout|Create workout|
|PUT|/workout/{id}|Update workout|
|DELETE|/workout/{id}|Delete workout|
|GET|/workout|List all workouts (pagination)|
|GET|/workout/{id}/users|List users assigned to workout|
|GET|/workout/{id}/goals|List goals linked to workout|
|GET|/workout/{id}/types|List types linked to workout|
|GET|/workout/{id}/modalities|List modalities linked to workout|
|GET|/workout/{id}/routines|List routines linked to workout|
|GET|/workout/{id}/exercises|List exercises linked to workout|
|GET|/workout/{id}/instructor|Get instructor of workout|

---
### Routine
**Description**: Manage workout routines.

|Method|Route|Description|
|---|---|---|
|GET|/routine/{id}|Get routine by ID|
|POST|/routine|Create routine|
|PUT|/routine/{id}|Update routine|
|DELETE|/routine/{id}|Delete routine|
|GET|/routine|List all routines (pagination)|
|GET|/routine/{id}/exercises|List exercises linked to routine|
|GET|/routine/{id}/workouts|List workouts linked to routine|
|GET|/routine/{id}/instructor|Get instructor of routine|

---
### Exercise
**Description**: Manage exercises and their variations.

|Method|Route|Description|
|---|---|---|
|GET|/exercise/{id}|Get exercise by ID|
|POST|/exercise|Create exercise|
|PUT|/exercise/{id}|Update exercise|
|DELETE|/exercise/{id}|Delete exercise|
|GET|/exercise|List all exercises (pagination)|
|GET|/exercise/{id}/workouts|List workouts linked to exercise|
|GET|/exercise/{id}/routines|List routines linked to exercise|
|GET|/exercise/{id}/instructor|Get instructor of exercise|
|GET|/exercise/{id}/variations|List variations linked to exercise|

---
## Dependency Injection (DI)
Dependency Injection (DI) is a core principle of EasyTrainer's architecture.

All components like Services, Repositories, UnitOfWork, and Middlewares are injected using the native .NET Core **IoC container**.

**Advantages of DI:**
- **Decouples** components from each other (loose coupling).
- **Improves testability** (easy mocking and unit testing).
- **Centralizes dependency management**.
- **Promotes the SOLID principles**, especially Dependency Inversion.

---
### Registration of Services
In the `Startup.cs` or `Program.cs` (depending on your .NET version), all services, repositories, and helpers are registered using `AddScoped`, `AddTransient`, or `AddSingleton`, according to the appropriate lifetime.

Example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Application Services
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IInstructorService, InstructorService>();
    services.AddScoped<IGoalService, GoalService>();
    services.AddScoped<ILevelService, LevelService>();
    services.AddScoped<ITypeService, TypeService>();
    services.AddScoped<IModalityService, ModalityService>();
    services.AddScoped<IHashtagService, HashtagService>();
    services.AddScoped<IWorkoutService, WorkoutService>();
    services.AddScoped<IRoutineService, RoutineService>();
    services.AddScoped<IExerciseService, ExerciseService>();

    // Relationship Services
    services.AddScoped<IUserHasGoalService, UserHasGoalService>();
    services.AddScoped<IWorkoutHasUserService, WorkoutHasUserService>();
    services.AddScoped<IRoutineHasExerciseService, RoutineHasExerciseService>();

    // Infrastructure
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IDeletionValidationService, DeletionValidationService>();

    // JWT Authentication setup
    services.AddAuthentication(options => { /* configure JWT options */ });

    // API Versioning, Swagger, Logging configuration etc.
}
```
