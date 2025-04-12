using Domain.Entities.Main;

namespace Domain.Entities.Relations
{
    public class WorkoutHasExercise
    {
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }

        public Workout Workout { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
