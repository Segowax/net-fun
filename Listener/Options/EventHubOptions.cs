namespace Listener.Options
{
    internal class EventHubOptions
    {
        public const string EventHub = "EventHub";

        public required string Namespace { get; set; }
        public required string HubName { get; set; }
    }
}
