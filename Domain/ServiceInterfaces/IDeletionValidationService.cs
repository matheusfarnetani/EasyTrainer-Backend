namespace Domain.ServiceInterfaces
{
    public interface IDeletionValidationService
    {
        Task<bool> CanDeleteTypeAsync(int typeId);
        Task<bool> CanDeleteModalityAsync(int modalityId);
        Task<bool> CanDeleteHashtagAsync(int hashtagId);
    }
}
