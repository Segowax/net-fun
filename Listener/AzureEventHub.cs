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
            try
            {
                _eventHubOptions = eventHubOptions;
                if (string.IsNullOrEmpty(_eventHubOptions.ConnectionString))
                {
                    try
                    {
                        _producerClient = new EventHubProducerClient(
                        _eventHubOptions.Namespace,
                        _eventHubOptions.HubName,
                        new DefaultAzureCredential(Credential.Options));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while creating the Event Hub Producer Client: {ex.Message}");
                        throw;
                    }
                }
                else
                {
                    _producerClient = new EventHubProducerClient(
                        _eventHubOptions.ConnectionString,
                        _eventHubOptions.HubName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the Event Hub Producer Client: {ex.Message}");
                throw;
            }
        }

        public async Task SendEvent(string microcontrollerData)
        {
            try
            {
                using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();
                var objectToSend = JsonSerializer.Deserialize<BaseSensorDataDto>(microcontrollerData);
                if (objectToSend != null)
                {
                    objectToSend.EnqueuedTime = DateTime.UtcNow;
                    var jsonToSend = JsonSerializer.Serialize(objectToSend);
                    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(jsonToSend))))
                    {
                        // if it is too large for the batch
                        throw new Exception($"Event {microcontrollerData} is too large for the batch and cannot be sent.");
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the Event Data Batch: {ex.Message}");
                throw;
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
