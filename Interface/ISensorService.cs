using Domain.DTOs;

namespace Interface
{
    public interface ISensorService
    {
        Task<IEnumerable<SensorDataDTO>> GetAllSensorData();
    }
}
