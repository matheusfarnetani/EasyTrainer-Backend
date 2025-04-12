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
- No dependency on Application, Infrastructure, or API layers.

### EasyTrainer.Infrastructure
- Implements database access using Entity Framework Core.
- Provides repository implementations.
- Manages database context and transactions via Unit of Work.
- Implements connection management for role-based database access.
- Depends on the **Domain** layer.

### EasyTrainer.Application
- Contains business logic for application use cases (application services).
- Defines DTOs to communicate between the API layer and the domain models.
- Implements services that orchestrate operations across repositories.
- Depends on the **Domain** layer.

### EasyTrainer.API
- Exposes the system through HTTP endpoints using Controllers.
- Handles input validation, authentication, authorization, and middleware execution.
- Delegates business processing to Application Services.
- Depends on the **Application** and **Infrastructure** layers.

---
## Main Patterns Used
|Pattern|Purpose|
|---|---|
|Clean Architecture|Separate concerns for better scalability and testability.|
|Domain-Driven Design (Lightweight)|Focus on core domain complexity using meaningful models.|
|Repository Pattern|Abstracts the data layer from the business layer.|
|Unit of Work Pattern|Manages multiple database operations as a single atomic transaction.|
|Service Layer Pattern|Encapsulates business logic outside of controllers.|
|DTO Pattern|Decouple internal models from external-facing API contracts.|

---
## Key Concepts
- **DTOs**: Structures used to transport data between layers, isolating the domain model from external systems.
- **Entities**: Represent business objects with identity, persistence, and behavior.
- **Repositories**: Provide data access operations abstracted away from database-specific concerns.
- **Unit of Work**: Controls transactional consistency across multiple repositories.
- **Middlewares**: Manage cross-cutting concerns such as error handling, authentication, and logging.
- **Authentication**: JWT-based security layer for authenticating and authorizing API access.
- **Pagination and Filtering**: All list responses are paginated and ready for scalability.
- **API Standardization**: All API responses follow a common `ApiResponseDTO<T>` envelope for consistency and better client handling.
