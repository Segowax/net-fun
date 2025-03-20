using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureConfigurations.AppConfigurations
{
    public static class AzureAppConfigurations
    {
        public static IConfigurationBuilder AddMyAzureAppConfigurations
            (this IConfigurationBuilder configurationBuilder, IConfiguration configuration)
        {
            configurationBuilder.AddAzureAppConfiguration(options =>
                options.Connect(
                    new Uri(configuration["AppConfig"]
                        ?? throw new ArgumentNullException("AppConfig")),
                    new DefaultAzureCredential(Credential.Options)));

            return configurationBuilder;
        }

        public static IServiceCollection RegisterMyAzureAppConfigurations(this IServiceCollection services)
        {
            services.AddAzureAppConfiguration();

            return services;
        }
    }
}
