using Domain.Entities.Main;
using Domain.Infrastructure.RepositoriesInterfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        private readonly AppDbContext _context;

        public VideoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Video>> GetFreshVideosAsync()
        {
            _context.ChangeTracker.Clear();
            return await _context.Videos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllWithNoTrackingAsync()
        {
            return await _context.Videos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetPendingVideosAsync()
        {
            return await _context.Videos
                .AsNoTracking()
                .Where(v => v.Status == 0)
                .ToListAsync();
        }

        public new async Task<Video?> GetByIdAsync(int id)
        {
            return await _context.Videos
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Videos.AnyAsync(v => v.Id == id);
        }

        public new async Task AddAsync(Video video)
        {
            await _context.Videos.AddAsync(video);
        }

        public new Task UpdateAsync(Video video)
        {
            _context.Entry(video).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<Video>> GetByUserIdAsync(int userId)
        {
            return await _context.Videos.Where(v => v.UserId == userId).OrderByDescending(v => v.UploadedAt).ToListAsync();
        }
    }
}
