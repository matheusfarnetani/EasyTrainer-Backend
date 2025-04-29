using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T>(AppDbContext databaseContext) : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _databaseContext = databaseContext;
        private readonly DbSet<T> _dbSet = databaseContext.Set<T>();

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                await SaveAsync();
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
