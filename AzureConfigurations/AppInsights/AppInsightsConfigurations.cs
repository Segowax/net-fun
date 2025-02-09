using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace AzureConfigurations.AppInsights
{
    public static class AppInsightsConfigurations
    {
        public static IServiceCollection RegisterMyAppInsightsConfigurations
            (this IServiceCollection services, string cloudRoleName)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<CustomTelemetryProccessor>();
            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>
                (provider => new CustomTelemetryInitializer(cloudRoleName: cloudRoleName));

            return services;
        }
    }
}
