using Application.Query;
using Application.QueryPattern;
using Domain.DTOs;
using Interface;

namespace Application.Queries;

public class GetCurrenLockSensorsState : IQuery<StringSensorDataDto> { }

public class GetCurrentLockStateHandler
    : IQueryHandler<GetCurrenLockSensorsState, StringSensorDataDto>
{
    public readonly ISensorService _sensorService;

    public GetCurrentLockStateHandler(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    public async Task<StringSensorDataDto?> HandleAsync(GetCurrenLockSensorsState query)
    {
        return await _sensorService.GetCurrentLockStateAsync();
    }
}
