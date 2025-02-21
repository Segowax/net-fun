using Azure.Identity;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using System.Text;
using Listener.Options;
using System.Text.Json;
using Domain.DTOs;

namespace Listener
{
    internal class AzureEventHub : IAsyncDisposable
    {
        private readonly EventHubProducerClient _producerClient;
        private readonly EventHubOptions _eventHubOptions;

        public AzureEventHub(EventHubOptions eventHubOptions)
        {
            _eventHubOptions = eventHubOptions;
            _producerClient = new EventHubProducerClient(
                _eventHubOptions.Namespace,
                _eventHubOptions.HubName,
                new DefaultAzureCredential(Credential.Options));
        }

        public async Task SendEvent()
        {
            if (BufforToSend.rs232Data.Count == 0)
            {
                return;
            }

            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();
            foreach (var itemToSend in BufforToSend.rs232Data)
            {
                var objectToSend = JsonSerializer.Deserialize<BaseSensorDataDto>(itemToSend.Value);
                if (objectToSend != null)
                {
                    objectToSend.EnqueuedTime = DateTime.UtcNow;
                    var jsonToSend = JsonSerializer.Serialize(objectToSend);
                    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(jsonToSend))))
                    {
                        // if it is too large for the batch
                        throw new Exception($"Event {itemToSend.Key} is too large for the batch and cannot be sent.");
                    }
                    else
                    {
                        BufforToSend.rs232Data.TryRemove(itemToSend.Key, out _);
                    }
                }
            }

            try
            {
                await _producerClient.SendAsync(eventBatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the event: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_producerClient != null)
            {
                await _producerClient.CloseAsync();
                await _producerClient.DisposeAsync();
            }
        }
    }
}
