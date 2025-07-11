using Application;
using Application.Queries;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SensorController> _logger;

        public SensorController(IMediator mediator, ILogger<SensorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetCurrentLockState")]
        public async Task<IActionResult> GetCurrentLockState()
        {
            try
            {
                _logger.LogWarning("GetCurrentLockState called");
                var result = await _mediator.SendAsync<GetCurrenLockSensorsState, StringSensorDataDto>
                    (new GetCurrenLockSensorsState());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTemperature")]
        public async Task<IActionResult> GetAllTemperatureData()
        {
            try
            {
                _logger.LogWarning("GetAllTemperatureData called");
                var result = await _mediator.SendAsync<GetTemperatureSensorData, IEnumerable<DoubleSensorDataDto>>
                    (new GetTemperatureSensorData());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
