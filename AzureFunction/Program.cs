using Application.IoC;
using AzureConfigurations.AppConfigurations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true);

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Configuration.AddMyAzureAppConfigurations(builder.Configuration);
builder.Services.RegisterApplicationLayerServices(builder.Configuration);

builder.Build().Run();
