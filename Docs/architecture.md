# Architecture
## Overview
EasyTrainerAPI is a modular, scalable, and cleanly organized backend architecture based on a combination of:
- **Clean Architecture**
- **Domain-Driven Design (DDD)** (lightweight)    
- **Repository and Unit of Work Patterns**
- **Service Layer Pattern**

The primary goals are low coupling, high cohesion, maintainability, testability, and clear separation of responsibilities.

---
## Solution Structure

```
/EasyTrainer/
├── /EasyTrainer.API/
│   ├── Controllers/
│   │   └── All controllers...
│   ├── Middleware/
│   │   └── Error handling, authentication, etc.
│   ├── Authentication/
│   │   └── JWT authentication logic
│   └── Program.cs
│
├── /EasyTrainer.Application/
│   ├── DTOs/
│   │   └── All Data Transfer Objects (DTOs)
│   ├── Services/
│   │   └── All service classes
│   └── Service Interfaces/
│       └── Interfaces for services
│
├── /EasyTrainer.Domain/
│   ├── Models/
│   │   └── Business objects (e.g., User, Exercise)
│   ├── Entities/
│   │   └── Core business entities with identity and behavior
│   └── Repository Interfaces/
│       └── Interfaces for data access
│
└── /EasyTrainer.Infrastructure/
    ├── Repository/
    │   └── Implementations of repositories
    ├── DbContext/
    │   └── EF Core DbContext
    └── UnitOfWork/
        └── Implementation of Unit of Work pattern
```

---
## Project Responsibilities
### EasyTrainer.Domain
- Core of the system: Entities, Models, Repository Interfaces.
- Contains pure business rules.
- Independent of frameworks or infrastructure.

### EasyTrainer.Infrastructure
- Implements database access using Entity Framework Core.
- Provides repository implementations.
- Manages database context and transactions via Unit of Work.
- Depends on **Domain**.

### EasyTrainer.Application
- Contains business logic for use cases (application services).
- Defines DTOs for communication between API and domain.
- Depends on **Domain**.

### EasyTrainer.API
- Exposes HTTP endpoints via Controllers.
- Handles incoming requests, authentication, error handling.
- Configures services, database connections, middleware.
- Depends on **Application** and **Infrastructure**.

---
## Main Patterns Used
|Pattern|Purpose|
|:--|:--|
|Clean Architecture|Separates concerns, promotes scalability and testability.|
|Domain-Driven Design (Light)|Focuses on the core domain logic and business terms.|
|Repository Pattern|Abstracts database access behind interfaces.|
|Unit of Work Pattern|Manages multiple database operations as a single transaction.|
|Service Layer Pattern|Centralizes business logic outside of controllers.|

---
## Key Concepts
- **DTOs**: Keep API contracts decoupled from internal models.
- **Entities**: Represent business objects with identity and behavior.
- **Repositories**: Provide data access methods without exposing database technology.
- **Unit of Work**: Allows batching multiple operations in a single transaction.
- **Middlewares**: Handle cross-cutting concerns like error handling and authentication.
- **Authentication**: JWT is used for securing API endpoints.
