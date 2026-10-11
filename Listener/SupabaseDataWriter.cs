using Listener.Models;
using Listener.Options;
using Supabase;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Listener
{
    internal class SupabaseDataWriter
    {
        internal readonly static ConcurrentDictionary<string, string> Error = [];
        private bool _isInitialized = false;
        private readonly Client _client;

        public SupabaseDataWriter(MySupabaseOptions options)
        {
            var customOptions = new SupabaseOptions { AutoConnectRealtime = false };
            var supabaseClient = new Client(options.ProjectURL, options.ApiKey, customOptions);

            _client = supabaseClient;
        }

        public async Task InitializeClient()
        {
            try
            {
                await _client.InitializeAsync();
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                Error.TryAdd(Guid.NewGuid().ToString(), $"Error during {nameof(InitializeClient)} - {ex.Message}");
            }
        }

        public async void SensorDataWrite(object? sender, KeyValuePair<string, string> args)
        {
            try
            {
                if (!_isInitialized)
                {
                    throw new Exception("Client is not initialized.");
                }

                var objectToSend = JsonSerializer.Deserialize<SensorData>(args.Value)
                    ?? throw new Exception("Error during deserialazing object");
                objectToSend.EnqueuedTime = DateTime.UtcNow;

                await _client.From<SensorData>().Insert(objectToSend);

                BufforToSend.rs232Data.Remove(args.Key);
            }
            catch (Exception ex)
            {
                Error.TryAdd(Guid.NewGuid().ToString(), $"Error during {nameof(SensorDataWrite)} - {ex.Message}");
            }
        }

        public void DisposeClient()
        {
            if (_isInitialized)
                _client.Auth.Shutdown();
        }
    }
}