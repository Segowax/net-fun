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

        public SensorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllSensorData()
        {
            try
            {
                var result = await _mediator.SendAsync<GetAllSensorData, IEnumerable<BaseSensorDataDto>>
                    (new GetAllSensorData());

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
