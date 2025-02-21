using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public abstract class BaseRepository<TEntity> :
        IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SensorContext _context;

        protected BaseRepository(SensorContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            _context.Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var data = _context.Set<TEntity>();

            return await data.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var data = await _context.FindAsync<TEntity>(id);

            return data;
        }

        public async Task<TEntity?> GetByNameAsync(string name)
        {
            var data = await _context.FindAsync<TEntity>(name);

            return data;
        }
    }
}
