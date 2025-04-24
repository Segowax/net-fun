using Domain.DTOs;

namespace Interface
{
    public interface ISensorService
    {
        Task<IEnumerable<BaseSensorDataDto>> GetAllSensorDataAsync();
        Task<IEnumerable<DoubleSensorDataDto>> GetTemperatureSensorDataAsync();
        Task<StringSensorDataDto?> GetCurrentLockStateAsync();
        Task SaveSensorDataAsync(BaseSensorDataDto data);
    }
}
