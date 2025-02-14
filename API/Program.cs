using Application.IoC;
using AzureConfigurations.AppConfigurations;
using AzureConfigurations.AppInsights;
using AzureConfigurations.EventHub;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddMyAzureAppConfigurations(builder.Configuration);
    builder.Configuration.AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);

    builder.Services.RegisterMyAzureAppConfigurations();
    builder.Services.RegisterMyAppInsightsConfigurations("API");
    builder.Services.RegisterEventHubConfiguration();

    builder.Services.RegisterApplicationLayerServices(builder.Configuration);

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<SensorContext>();
        await context.Database.MigrateAsync();
    }

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
