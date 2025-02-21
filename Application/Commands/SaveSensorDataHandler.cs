using Application.CommandPattern;
using Domain.DTOs;
using Interface;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class SaveSensorData : ICommand
    {
        public required BaseSensorDataDto Data { get; set; }
    }

    public class SaveSensorDataHandler :
        ICommandHandler<SaveSensorData>
    {
        private readonly ISensorService _sensorService;
        private readonly ILogger<SaveSensorDataHandler> _logger;

        public SaveSensorDataHandler(ISensorService sensorService,
            ILogger<SaveSensorDataHandler> logger)
        {
            _sensorService = sensorService;
            _logger = logger;
        }

        public async Task HandleAsync(SaveSensorData command)
        {
            try
            {
                await _sensorService.SaveSensorData(command.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving sensor data");
                throw;
            }
        }
    }
}
