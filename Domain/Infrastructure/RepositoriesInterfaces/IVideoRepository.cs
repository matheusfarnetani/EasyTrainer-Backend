using Domain.Entities.Main;

namespace Domain.Infrastructure.RepositoriesInterfaces
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<IEnumerable<Video>> GetFreshVideosAsync();
        Task<IEnumerable<Video>> GetAllWithNoTrackingAsync();
        Task<IEnumerable<Video>> GetPendingVideosAsync();
        new Task<Video?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        new Task AddAsync(Video video);
        new Task UpdateAsync(Video video);
        Task<IEnumerable<Video>> GetByUserIdAsync(int userId);
    }
}
