namespace Application.Services.Interfaces
{
    public interface IDeletionValidationService
    {
        Task<bool> CanDeleteTypeAsync(int typeId, int instructorId);
        Task<bool> CanDeleteModalityAsync(int modalityId, int instructorId);
        Task<bool> CanDeleteHashtagAsync(int hashtagId, int instructorId);
        Task<bool> CanDeleteGoalAsync(int goalId, int instructorId);
    }
}