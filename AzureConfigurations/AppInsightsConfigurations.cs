using Microsoft.Extensions.DependencyInjection;

namespace AzureConfigurations
{
    public static class AppInsightsConfigurations
    {
        public static IServiceCollection RegisterMyAppInsightsConfigurations(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}
