using Azure.Identity;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using System.Text;
using Listener.Options;

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
            // Create a batch of events 
            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync();

            foreach (var itemToSend in BufforToSend.rs232Data)
            {
                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(itemToSend.Value))))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {itemToSend.Key} is too large for the batch and cannot be sent.");
                }
                else
                {
                    BufforToSend.rs232Data.TryRemove(itemToSend.Key, out _);
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
                await _producerClient.DisposeAsync();
            }
        }
    }
}
