using Domain.DTOs;

namespace Interface
{
    public interface ISensorService
    {
        Task<IEnumerable<BaseSensorDataDto>> GetAllSensorData();
        Task SaveSensorData(BaseSensorDataDto data);
    }
}
