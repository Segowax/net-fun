using Application.CommandPattern;
using Application.Commands;
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
        services.AddScoped<IQueryHandler<GetAllSensorData, IEnumerable<BaseSensorDataDto>>, GetAllSensorDataHandler>();
        services.AddScoped<IQueryHandler<GetTemperatureSensorData, IEnumerable<DoubleSensorDataDto>>, GetTemperatureSensorDataHandler>();

        // Explicitly register Commands
        services.AddScoped<ICommandHandler<SaveSensorData>, SaveSensorDataHandler>();

        return services;
    }
}
