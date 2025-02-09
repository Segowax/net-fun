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
#if DEBUG
            var conn = configuration["AzureAppConfigurationConnectionString"] ??
                throw new ArgumentNullException("AzureAppConfigurationConnectionString");
            configurationBuilder.AddAzureAppConfiguration(options => options.Connect(conn));
#else
            configurationBuilder.AddAzureAppConfiguration(options =>
                options.Connect(
                    new Uri(configuration["AppConfig"] ?? throw new ArgumentNullException("AppConfig")),
                    new ManagedIdentityCredential()));
#endif

            return configurationBuilder;
        }

        public static IServiceCollection RegisterMyAzureAppConfigurations(this IServiceCollection services)
        {

            services.AddAzureAppConfiguration();

            return services;
        }
    }
}
