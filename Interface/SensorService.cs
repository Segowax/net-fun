using Domain.DTOs;
using Interface.Mapper;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interfaces;

namespace Interface
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ILogger<SensorService> _logger;

        public SensorService(ISensorRepository sensorRepository,
            ILogger<SensorService> logger)
        {
            _sensorRepository = sensorRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BaseSensorDataDto>> GetAllSensorDataAsync()
        {
            try
            {
                var result = await _sensorRepository.GetAllAsync();

                return result
                    .ToList()
                    .ConvertAll(x => x.MapSensorDataToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetAllSensorDataAsync)} - Error getting all sensor data");
                throw;
            }
        }

        public async Task<IEnumerable<DoubleSensorDataDto>> GetTemperatureSensorDataAsync()
        {
            try
            {
                var result = await _sensorRepository.GetAllAsync();

                return result
                    .Where(x => x.SensorId.Contains("temperature"))
                    .ToList()
                    .ConvertAll(x => x.MapToDoubleSensorDataDto())
                    .Where(x => x is not null)!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetTemperatureSensorDataAsync)} - Error getting temperature sensor data");
                throw;
            }
        }

        public async Task SaveSensorDataAsync(BaseSensorDataDto data)
        {
            try
            {
                var dataToSave = data.MapDtoToSensorDataEntity();
                await _sensorRepository.AddAsync(dataToSave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SaveSensorDataAsync)} - Error saving sensor data");
                throw;
            }
        }
    }
}
