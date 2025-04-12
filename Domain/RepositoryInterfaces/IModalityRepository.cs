﻿using Domain.Entities.Main;

namespace Domain.RepositoryInterfaces
{
    public interface IModalityRepository : IGenericRepository<Modality>
    {
        Task<IEnumerable<Modality>> GetModalitiesByWorkoutAsync(int workoutId);
        Task<IEnumerable<Modality>> GetModalitiesByRoutineAsync(int routineId);
        Task<IEnumerable<Modality>> GetModalitiesByExerciseAsync(int exerciseId);
    }
}
