using Application.Queries;
using Application.Query;
using Domain.DTOs;
using Interface.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.IoC;

namespace Application.IoC;

public static class RegisterServices
{
    public static IServiceCollection RegisterApplicationLayerServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRepistoryLayerServices(configuration);
        services.RegisterServiceLayerServices();

        services.AddScoped<IMediator, Mediator>();

        // Explicitly register Queries
        services.AddScoped<IQueryHandler<GetAllSensorData, IEnumerable<SensorDataDto>>, GetAllSensorDataHandler>();

        return services;
    }
}
