using Application.Query;
using Application.QueryPattern;
using Domain.DTOs;
using Interface;

namespace Application.Queries;

public class GetAllSensorData : IQuery<IEnumerable<SensorDataDto>> { }

public class GetAllSensorDataHandler :
    IQueryHandler<GetAllSensorData, IEnumerable<SensorDataDto>>
{
    public readonly ISensorService _sensorService;

    public GetAllSensorDataHandler(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    public async Task<IEnumerable<SensorDataDto>> HandleAsync(GetAllSensorData query)
    {
        return await _sensorService.GetAllSensorData();
    }
}
