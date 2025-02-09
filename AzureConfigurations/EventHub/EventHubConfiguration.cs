using Microsoft.Extensions.DependencyInjection;

namespace AzureConfigurations.EventHub
{
    public static class EventHubConfiguration
    {
        public static IServiceCollection RegisterEventHubConfiguration(this IServiceCollection services)
        {
            services.AddHostedService<EventHubConsumerService>();

            return services;
        }
    }
}
