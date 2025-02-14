using Application.Query;
using Domain.DTOs;
using Interface;

namespace Application
{
    public class GetAllSensorData : IQuery<IEnumerable<SensorDataDTO>> { }

    public class GetAllSensorDataHandler :
        IQueryHandler<GetAllSensorData, IEnumerable<SensorDataDTO>>
    {
        public readonly ISensorService _sensorService;

        public GetAllSensorDataHandler(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        public async Task<IEnumerable<SensorDataDTO>> Handle(GetAllSensorData query)
        {
            return await _sensorService.GetAllSensorData();
        }
    }
}
