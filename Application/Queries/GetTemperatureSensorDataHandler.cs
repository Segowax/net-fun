using Application.Query;
using Application.QueryPattern;
using Domain.DTOs;
using Interface;

namespace Application.Queries
{
    public class GetTemperatureSensorData : IQuery<IEnumerable<DoubleSensorDataDto>> { }

    public class GetTemperatureSensorDataHandler :
        IQueryHandler<GetTemperatureSensorData, IEnumerable<DoubleSensorDataDto>>
    {
        public readonly ISensorService _sensorService;

        public GetTemperatureSensorDataHandler(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        public async Task<IEnumerable<DoubleSensorDataDto>?> HandleAsync(GetTemperatureSensorData query)
        {
            return await _sensorService.GetTemperatureSensorDataAsync();
        }
    }
}
