using Application;
using Application.Commands;
using Azure.Identity;
using Azure.Messaging.EventHubs.Consumer;
using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace AzureConfigurations.EventHub
{
    public class EventHubConsumerService : BackgroundService
    {
        private readonly string _eventHubNamespace;
        private readonly string _eventHubName;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EventHubConsumerService> _logger;
        private EventHubConsumerClient? _consumerClient;

        public EventHubConsumerService(IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<EventHubConsumerService> logger)
        {
            _eventHubNamespace = configuration["EventHub:Namespace"]
                ?? throw new ArgumentNullException("EventHub:Namespace is not set in App Configurations");
            _eventHubName = configuration["EventHub:Name"]
                ?? throw new ArgumentNullException("EventHub:Name is not set in App Configurations");
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _consumerClient = new EventHubConsumerClient(
                    EventHubConsumerClient.DefaultConsumerGroupName,
                    _eventHubNamespace,
                    _eventHubName,
                    new DefaultAzureCredential(Credential.Options));

                return base.StartAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.CompletedTask;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_consumerClient != null)
            {
                await _consumerClient.CloseAsync();
                await _consumerClient.DisposeAsync();
            }
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (PartitionEvent partitionEvent in _consumerClient!.ReadEventsAsync())
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var jsonEventData = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                        var sensorData = JsonSerializer.Deserialize<BaseSensorDataDto>(jsonEventData);

                        if (sensorData != null)
                        {
                            await mediator.SendAsync(new SaveSensorData { Data = sensorData });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Błąd podczas odbierania zdarzeń: {ex.Message}");
            }
        }
    }
}
