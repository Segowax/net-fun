using Application.Commands;
using Application;
using Azure.Messaging.EventHubs;
using Domain.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace MyWonderful.Function
{
    public class MyWonderfulEventHubTrigger
    {
        private readonly ILogger<MyWonderfulEventHubTrigger> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MyWonderfulEventHubTrigger(IServiceScopeFactory serviceScopeFactory,
            ILogger<MyWonderfulEventHubTrigger> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        [Function(nameof(MyWonderfulEventHubTrigger))]
        public async Task Run(
            [EventHubTrigger("my-wonderful-event-hub",
            Connection = "mywonderfuleventhubnamespace_RootManageSharedAccessKey_EVENTHUB")]
        EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var jsonEventData = Encoding.UTF8.GetString(@event.EventBody.ToArray());
                        var sensorData = JsonSerializer.Deserialize<BaseSensorDataDto>(jsonEventData);

                        if (sensorData != null)
                        {
                            await mediator.SendAsync(new SaveSensorData { Data = sensorData });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error during receiving event: {ex.Message}");
                }
            }
        }
    }
}
