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
### 1.  user
**Properties**
- id *int*
- name *string*  
- email *string*  
- mobile_number *string*  
- birthday *date*  
- weight *float*  
- height *float*  
- gender *char*  
- password *string*
- level_id *int*
- instructor_id *int*

**Navigation Properties**
- ICollection of UserHasGoal - UserGoals
- ICollection of WorkoutHasUser - WorkoutUsers

---
### 2. instructor
**Properties**
- id *int*
- name *string*  
- email  *string*  
- mobile_number *string*  
- birthday *date*  
- gender *char*  
- password *string*

**Navigation Properties**
- ICollection of Workout - Workouts
- ICollection of Routine - Routines
- ICollection of Exercise - Exercises

---
### 3. goal
**Properties**
- id *int*
- name *string*  
- description *string*

**Navigation Properties**
- ICollection of UserHasGoal - UserGoals
- ICollection of WorkoutHasGoal - WorkoutGoals
- ICollection of RoutineHasGoal - RoutineGoals
- ICollection of ExerciseHasGoal - ExerciseGoals

---
### 4. level
**Properties**
- id *int*
- name *string*  
- description *string*

**Navigation Properties**
- ICollection of User - Users
- ICollection of Workout - Workouts
- ICollection of Routine - Routines
- ICollection of Exercise - Exercises

---
### 5. type
**Properties**
- id *int*
- name *string*  
- description *string*

**Navigation Properties**
- ICollection of WorkoutHasType - WorkoutTypes
- ICollection of RoutineHasType - RoutineTypes
- ICollection of ExerciseHasType - ExerciseTypes

---
### 6. modality
**Properties**
- id *int*
- name *string*  
- description *string*

**Navigation Properties**
- ICollection of WorkoutHasModality - WorkoutModalities
- ICollection of RoutineHasModality - RoutineModalities
- ICollection of ExerciseHasModality - ExerciseModalities

---
### 7. hashtag
**Properties**
- id *int*
- hashtag *string*

**Navigation Properties**
- ICollection of WorkoutHasHashtag - WorkoutHashtags
- ICollection of RoutineHasHashtag - RoutineHashtags
- ICollection of ExerciseHasHashtag - ExerciseHashtags

---
### 8. workout
**Properties**
- id *int*
- name *string*  
- description *string*  
- number_of_days *int*  
- image_url *string*  
- duration *time*  
- indoor *bool*
- instructor_id *int*
- level_id *int*

**Navigation Properties**
- ICollection of WorkoutHasType - WorkoutTypes
- ICollection of WorkoutHasModality - WorkoutModalities
- ICollection of WorkoutHasGoal - WorkoutGoals
- ICollection of WorkoutHasHashtag - WorkoutHashtags
- ICollection of WorkoutHasUser - WorkoutUsers
- ICollection of WorkoutHasRoutine - WorkoutRoutines
- ICollection of WorkoutHasExercise - WorkoutExercises

---
### 9. routine
**Properties**
- id *int*
- name *string*  
- description *string*  
- duration *time*  
- image_url *string*
- instructor_id *int*
- level_id *int*

**Navigation Properties**
- ICollection of RoutineHasType - RoutineTypes
- ICollection of RoutineHasModality - RoutineModalities
- ICollection of RoutineHasGoal - RoutineGoals
- ICollection of RoutineHasHashtag - RoutineHashtags
- ICollection of RoutineHasExercise - RoutineExercises
- ICollection of WorkoutHasRoutine - WorkoutRoutines

---
### 10. exercise
**Properties**
- id *int*
- name *string*  
- description *string*  
- equipment *string*  
- duration *time*  
- repetition *int*  
- sets *int*  
- rest_time *time*  
- body_part *string*  
- video_url *string*  
- image_url *string*  
- steps (instructions) *string*  
- contraindications *string*  
- safety_tips *string*  
- common_mistakes *string*  
- indicated_for *string*  
- calories_burned_estimate *float*
- instructor_id *int*
- level_id *int*

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

### 11. UserHasGoal
**Properties**
- user_id *int*  
- goal_id *int*


**Navigation Properties**
- User
- Goal

---
### 12. WorkoutHasType
**Properties**
- workout_id *int*  
- type_id *int*


**Navigation Properties**
- Workout
- Type

---
### 13. WorkoutHasModality
**Properties**
- workout_id *int*  
- modality_id *int*


**Navigation Properties**
- Workout
- Modality

---
### 14. WorkoutHasGoal
**Properties**
- workout_id *int*  
- goal_id *int*


**Navigation Properties**
- Workout
- Goal

---
### 15. WorkoutHasHashtag
**Properties**
- workout_id *int*  
- hashtag_id *int*


**Navigation Properties**
- Workout
- Hashtag

---
### 16. WorkoutHasUser
**Properties**
- workout_id  *int*
- user_id *int*


**Navigation Properties**
- Workout
- User

---
### 17. WorkoutHasRoutine
**Properties**
- workout_id  *int*
- routine_id *int*


**Navigation Properties**
- Workout
- Routine

---
### 18. WorkoutHasExercise
**Properties**
- workout_id  *int*
- exercise_id *int*


**Navigation Properties**
- Workout
- Exercise

---
### 19. RoutineHasType
**Properties**
- routine_id *int*  
- type_id *int*

**Navigation Properties**
- Routine
- Type

---
### 20. RoutineHasModality
**Properties**
- routine_id *int*  
- modality_id *int*

**Navigation Properties**
- Routine
- Modality

---
### 21. RoutineHasGoal
**Properties**
- routine_id *int*  
- goal_id *int*

**Navigation Properties**
- Routine
- Goal

---
### 22. RoutineHasExercise
**Properties**
- routine_id *int*  
- exercise_id *int*  
- order *int*  
- sets *int*  
- reps *int*  
- rest_time *time*  
- note *string*  
- day *int*  
- week *int*  
- is_optional *bool*

**Navigation Properties**
- Routine
- Exercise

---
### 23. RoutineHasHashtag
**Properties**
- routine_id *int*  
- hashtag_id *int*

**Navigation Properties**
- Routine
- Hashtag

---
### 24. ExerciseHasType
**Properties**
- exercise_id *int*  
- type_id *int*

**Navigation Properties**
- Exercise
- Type

---
### 25. ExerciseHasModality
**Properties**
- exercise_id *int*  
- modality_id *int*

**Navigation Properties**
- Exercise
- Modality

---
### 26. ExerciseHasGoal
**Properties**
- exercise_id *int*  
- goal_id *int*

**Navigation Properties**
- Exercise
- Goal

---
### 27. ExerciseHasHashtag
**Properties**
- exercise_id *int*  
- hashtag_id *int*

**Navigation Properties**
- Exercise
- Hashtag

---
### 28. ExerciseHasVariation
**Properties**
- exercise_id *int*  
- variation_id *int*

**Navigation Properties**
- Exercise
- Variation

---
## Value Objects (Optional for future)
- Value Objects represent attributes that do not have identity.
- Currently not used, but examples could include `Address`, `ContactInformation`, etc., if necessary in future domain expansions.

---
## Repositories Interfaces
All main domain models have their own repository and repository interface, following the generic repository pattern.

### Main Repositories Interfaces
- IUserRepository
- IInstructorRepository
- IGoalRepository
- ILevelRepository
- ITypeRepository
- IModalityRepository
- IHashtagRepository
- IWorkoutRepository
- IRoutineRepository
- IExerciseRepository

### Relationship Repositories Interfaces
- IRoutineHasExerciseRepository

### Notes
- Only `RoutineHasExercise` has a dedicated repository because it contains additional business attributes (order, sets, reps, rest time, notes, day, week, is_optional).
- All other relationship tables (simple many-to-many links) are handled through entity navigation and do not require separate repositories.

---
### Generic Repository
The **Generic Repository Pattern** is designed to provide a reusable set of data operations (CRUD) for different entities without duplicating code.

The logic behind it is:
- Define a **generic interface** `IRepository` that works with any entity type.
- Implement **standard CRUD operations** once, and reuse them for all entities.
- Extend the generic repository with **specific repositories** if you need custom queries (e.g., `GetUserByEmail`, `GetWorkoutsByUserId`).

**Benefits of using a Generic Repository:**
- Reduces code duplication by centralizing common CRUD operations.
- Ensures consistency across all data access layers.
- Simplifies maintenance by isolating data access logic.
- Facilitates scalability when adding new entities.
- Promotes separation of concerns between business logic and data persistence.
