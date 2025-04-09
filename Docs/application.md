# Application
## DTOs
- Data Transfer Objects (DTOs) are simple objects used to encapsulate data sent between the API and the application layer.
- Separate **Input DTOs** and **Output DTOs** for each Main Model.

### Inputs
- DTOs used for **Create** and **Update** operations.
- Contain only fields allowed to be written by the client.
- May include validation annotations (e.g., `[Required]`, `[MaxLength]`).
    - CreateUserInputDTO
    - UpdateUserInputDTO
    - CreateInstructorInputDTO
    - UpdateInstructorInputDTO
    - CreateGoalInputDTO
    - CreateLevelInputDTO
    - CreateTypeInputDTO
    - CreateModalityInputDTO
    - CreateHashtagInputDTO
    - CreateWorkoutInputDTO
    - UpdateWorkoutInputDTO
    - CreateRoutineInputDTO
    - UpdateRoutineInputDTO
    - CreateExerciseInputDTO
    - UpdateExerciseInputDTO
    - CreateUserHasGoalInputDTO
    - CreateWorkoutHasUserInputDTO
    - CreateRoutineHasExerciseInputDTO

### Outputs
- DTOs used for **Read** operations.
- May expose additional computed fields (e.g., `FullName`, `GoalNames`, etc.).
- Never expose sensitive fields (e.g., passwords).
- **Unit**
    - UserOutputDTO
    - InstructorOutputDTO
    - GoalOutputDTO
    - LevelOutputDTO
    - TypeOutputDTO
    - ModalityOutputDTO
    - HashtagOutputDTO
    - WorkoutOutputDTO
    - RoutineOutputDTO
    - ExerciseOutputDTO
    - UserHasGoalOutputDTO
    - WorkoutHasUserOutputDTO
    - RoutineHasExerciseOutputDTO

- **List**
    - UserListOutputDTO
    - InstructorListOutputDTO
    - WorkoutListOutputDTO
    - RoutineListOutputDTO
    - ExerciseListOutputDTO
    - VariationsOutputDTO

---
## Services
- Services handle **application business logic** and **orchestrate operations**.
- Services should be **stateless** and **only depend on repositories or other services**.
- Each main model should have its corresponding Service and Service Interface.

### Responsibilities
- Perform **validations** before calling the database.
- Handle **rules** and **restrictions** (e.g., cannot create a Workout without an Instructor).
- Combine **multiple repositories** when necessary.
- Handle **transactions** (possibly via UnitOfWork).

### Services Interfaces
- IUserService
	- CreateUser(*CreateUserInputDTO*) -> *UserOutputDTO*
	- UpdateUser(*UpdateUserInputDTO*) -> *UserOutputDTO*
	- DeleteUser(*int* userId) -> *Void*
	- GetUserById(*int* userId) -> *UserOutputDTO*
	- ListUsers() -> List of *UserListOutputDTO*
- IInstructorService
	- CreateInstructor(*CreateInstructorInputDTO*) ->*InstructorOutputDTO*
	- UpdateInstructor(*UpdateInstructorInputDTO*) -> *InstructorOutputDTO*
	- DeleteInstructor(*int* instructorId) ->*Void*
	- GetInstructorById(*int* instructorId) -> *InstructorOutputDTO*
	- ListInstructors() -> List of *InstructorListOutputDTO*
- IGoalService
	- CreateGoal(*CreateGoalInputDTO*) -> *GoalOutputDTO*
	- DeleteGoal(*int* goalId) -> *Void*
	- GetGoalById(*int* goalId) -> *GoalOutputDTO*
	- GetGoals() -> List of *GoalOutputDTO*
- ILevelService
	- CreateLevel(*CreateLevelInputDTO*) -> *LevelOutputDTO*
	- DeleteLevel(*int* levelId) -> *Void*
	- GetLevelById(*int* levelId) -> *LevelOutputDTO*
	- GetLevels() -> List of *LevelOutputDTO*
- ITypeService
	- CreateType(*CreateTypeInputDTO*) -> *TypeOutputDTO*
	- DeleteType(*int* typeId) -> *Void*
	- GetTypeById(*int* typeId) -> *TypeOutputDTO*
	- GetTypes() -> List of *TypeOutputDTO*
- IModalityService
	- CreateModality(*CreateModalityInputDTO*) -> *ModalityOutputDTO*
	- DeleteModality(*int* modalityId) -> *Void*
	- GetModalityById(*int* modalityId) -> *Void*
	- GetModalities() -> List of *ModalityOutputDTO*
- IHashtagService
	- CreateHashtag(*CreateHashtagInputDTO*) -> *HashtagOutputDTO*
	- DeleteHashtag(*int* hashtagId) -> *Void*
	- GetHashtagById(*int* hashtagId) -> *HashtagOutputDTO*
	- GetHashtags() -> List of *HashtagOutputDTO*
- IWorkoutService
	- CreateWorkout(*CreateWorkoutInputDTO*) -> *WorkoutOutputDTO*
	- UpdateWorkout(*UpdateWorkoutInputDTO*) -> *WorkoutOutputDTO*
	- DeleteWorkout(*int* workoutId) -> *Void*
	- GetWorkoutById(*int* workoutId) -> *WorkoutOutputDTO*
	- ListWorkouts() -> List of *WorkoutListOutputDTO*
- IRoutineService
	- CreateRoutine(*CreateRoutineInputDTO*) -> *RoutineOutputDTO*
	- UpdateRoutine(*UpdateRoutineInputDTO*) -> *RoutineOutputDTO*
	- DeleteRoutine(*int* routineId) -> *Void*
	- GetRoutineById(*int* routineId) -> *RoutineOutputDTO*
	- ListRoutines() -> List of *RoutineListOutputDTO*
- IExerciseService
	- CreateExercise(*CreateExerciseInputDTO*) -> *ExerciseOutputDTO*
	- UpdateExercise(*UpdateExerciseInputDTO*) -> *ExerciseOutputDTO*
	- DeleteExercise(*int* exerciseId) -> *Void*
	- GetExerciseById(*int* exerciseId) -> *ExerciseOutputDTO*
	- AddVariationToExercise(*int* exerciseId, *int* variationExerciseId) -> *Void*
	- RemoveVariationFromExercise(*int* exerciseId, *int* variationExerciseId) -> *Void*
	- ListExercises() -> List of *ExerciseListOutputDTO*
	- ListVariations(*int* exerciseId) -> List of *VariationsOutputDTO*
- IUserHasGoalService
	- CreateUserHasGoal(*CreateUserHasGoalInputDTO*) -> *UserHasGoalOutputDTO*
	- DeleteUserHasGoal(*int* userHasGoalId) -> *Void*
- IWorkoutHasUserService
	- CreateWorkoutHasUser(*CreateWorkoutHasUserInputDTO*) -> *WorkoutHasUserOutputDTO*
	- DeleteWorkoutHasUser(*int* workoutHasUserId) -> *Void*
- IRoutineHasExerciseService
	- CreateRoutineHasExercise(*CreateRoutineHasExerciseInputDTO*) -> *RoutineHasExerciseOutputDTO*
	- DeleteRoutineHasExercise(*int* routineHasExerciseId) -> *Void*

### Services Implementation
- UserService
- InstructorService
- GoalService
- LevelService
- TypeService
- ModalityService
- HashtagService
- WorkoutService:
- RoutineService
- ExerciseService:
- UserHasGoalService
- WorkoutHasUserService
- RoutineHasExerciseService

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
