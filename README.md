# EasyTrainer API
Welcome to the **EasyTrainer API** project — a clean, modular, and scalable backend architecture designed for managing users, workouts, exercises, instructors, and training routines.

This repository follows modern backend principles including:
- **Clean Architecture**
- **Domain-Driven Design (Lightweight DDD)**
- **Repository and Unit of Work Patterns**
- **Service Layer Pattern**
- **Fully documented and ready for scalable deployments**

---
## Project Documentation
The full project documentation is separated into clear sections for easy navigation:

| Section         | Description                                    | Link |
|:----------------|:-----------------------------------------------|:----|
| **Architecture** | System organization, project structure, main patterns | [Architecture](./Docs/architecture.md) |
| **Domain**       | Core business models, entities, relationships | [Domain](./Docs/domain.md) |
| **Infrastructure** | Database access, repositories, connection manager | [Infrastructure](./Docs/infrastructure.md) |
| **Application**  | DTOs, Services, Unit of Work, AutoMapper profiles | [Application](./Docs/application.md) |
| **API**          | Routes, controllers, middlewares, API philosophy | [API](./Docs/api.md) |

---
## Database
The database schema, scripts, triggers, and additional configurations for EasyTrainer are managed in a separate repository:
- [EasyTrainer-Database Repository](https://github.com/matheusfarnetani/EasyTrainer-Database/)

---
## Getting Started (Coming Soon)
- API setup instructions
- Local development environment
- Running with Docker (optional future step)

---
## Roadmap
The full project documentation is separated into clear sections for easy navigation:

| Order | Step | Status |
|:-----:|:-----|:------|
| 1 | Set up the development environment (.NET, MySQL, auxiliary tools) | ✅ |
| 2 | Create the folder and project structure according to the architecture documentation | ✅ |
| 3 | Implement the Entities and Models in **Domain** | ✅ |
| 4 | Define and implement all **Repository Interfaces** and **Service Interfaces** in **Domain** | ✅ |
| 5 | Implement **DTOs** in **Application** | 🔲 |
| 6 | Implement **Application Services** in **Application** | 🔲 |
| 7 | Implement the **Generic Repository** and **Specific Repositories** in **Infrastructure** | 🔲 |
| 8 | Implement the **ConnectionManager** in **Infrastructure** | 🔲 |
| 9 | Implement the **Unit of Work** in **Infrastructure** | 🔲 |
| 10 | Create **Middlewares** in the API (Error Handling, Authentication, Logging) | 🔲 |
| 11 | Configure **JWT Authentication** in the API | 🔲 |
| 12 | Create all **Controllers** in the API following the architecture standards | 🔲 |
| 13 | Configure **Dependency Injection** in `Program.cs` | 🔲 |
| 14 | Implement **API Versioning** | 🔲 |
| 15 | (Optional) Configure **Swagger (OpenAPI)** for automatic API documentation | 🔲 |
| 16 | Test the main flows (CRUD operations for users, instructors, workouts, etc.) | 🔲 |
| 17 | Plan future integrations (e.g., Caching, Rate-Limiting, Event Bus) | 🔲 |

### Notes
- **Swagger** is optional and can be added later to assist API testing.
- Every layer should be developed and validated **in isolation** to ensure low coupling.
- Integration testing will be easier if the architecture is respected from the beginning.

---
## Contributions
This is an academic, experimental, and learning project. Contributions and feedback are always welcome!

---
## License
**MIT License** — Feel free to use and adapt with proper attribution.
