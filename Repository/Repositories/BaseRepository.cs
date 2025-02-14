using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public abstract class BaseRepository<Entity> :
        IBaseRepository<Entity> where Entity : class
    {
        private readonly SensorContext _context;

        protected BaseRepository(SensorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entity>> GetAllAsync()
        {
            var data = _context.Set<Entity>();

            return await data.ToListAsync();
        }

        public async Task<Entity?> GetByIdAsync(int id)
        {
            var data = await _context.FindAsync<Entity>(id);

            return data;
        }

        public async Task<Entity?> GetByNameAsync(string name)
        {
            var data = await _context.FindAsync<Entity>(name);

            return data;
        }
    }
}
