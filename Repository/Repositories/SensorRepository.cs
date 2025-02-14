using Domain.Entities;
using Repository.Context;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SensorRepository : BaseRepository<SensorData>, ISensorRepository
    {
        public SensorRepository(SensorContext context) : base(context) { }
    }
}
