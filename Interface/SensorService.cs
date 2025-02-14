using Domain.DTOs;
using Interface.Mapper;
using Repository.Repositories.Interfaces;

namespace Interface
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorService(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public async Task<IEnumerable<SensorDataDTO>> GetAllSensorData()
        {
            var result = await _sensorRepository.GetAllAsync();

            return result
                .ToList()
                .ConvertAll(x => x.MapSensorDataToDto());
        }
    }
}
