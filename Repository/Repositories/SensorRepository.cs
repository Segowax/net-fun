using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SensorRepository : BaseRepository<SensorData>, ISensorRepository
    {
        private readonly SensorContext _context;

        public SensorRepository(SensorContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SensorData?> GetCurrentLockStateAsync()
        {
            var data = await _context.SensorData
                .Where(x => x.Name.Contains("Lock"))
                .OrderByDescending(x => x.EnqueuedTime)
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
