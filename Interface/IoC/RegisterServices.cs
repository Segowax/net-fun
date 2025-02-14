using Microsoft.Extensions.DependencyInjection;

namespace Interface.IoC
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterServiceLayerServices
            (this IServiceCollection services)
        {
            services.AddScoped<ISensorService, SensorService>();

            return services;
        }
    }
}
