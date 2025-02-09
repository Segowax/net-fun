using Azure.Identity;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureConfigurations.EventHub
{
    public class EventHubConsumerService : BackgroundService
    {
        private readonly string _eventHubNamespace;
        private readonly string _eventHubName;
        private readonly ILogger<EventHubConsumerService> _logger;
        private EventHubConsumerClient? _consumerClient;

        public EventHubConsumerService(IConfiguration configuration,
            ILogger<EventHubConsumerService> logger)
        {
            _eventHubNamespace = configuration["EventHub:Namespace"]
                ?? throw new ArgumentNullException("EventHub:Namespace is not set in App Configurations");
            _eventHubName = configuration["EventHub:Name"]
                ?? throw new ArgumentNullException("EventHub:Name is not set in App Configurations");
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
                    new ManagedIdentityCredential());

                return base.StartAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.CompletedTask;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (PartitionEvent partitionEvent in _consumerClient!.ReadEventsAsync())
                {
                    _logger.LogInformation($"Odebrano zdarzenie z partycji: {partitionEvent.Partition.PartitionId}");
                    _logger.LogInformation($"Treść zdarzenia: {System.Text.Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray())}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Błąd podczas odbierania zdarzeń: {ex.Message}");
            }
            finally
            {
                await _consumerClient!.CloseAsync();
            }
        }
    }
}
