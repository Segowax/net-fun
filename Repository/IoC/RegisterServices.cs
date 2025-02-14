using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Context;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository.IoC
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterRepistoryLayerServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISensorRepository, SensorRepository>();

            services.AddSqlServer<SensorContext>(
                connectionString: configuration.GetConnectionString("SensorContext"));

            return services;
        }
    }
}
