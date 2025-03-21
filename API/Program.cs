using API;
using Application.IoC;
using AzureConfigurations.AppConfigurations;
using AzureConfigurations.AppInsights;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddEnvironmentVariables();
    builder.Configuration.AddMyAzureAppConfigurations(builder.Configuration);
    builder.Configuration.AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);

    builder.Services.RegisterMyAzureAppConfigurations();
    builder.Services.RegisterMyAppInsightsConfigurations("API");

    builder.Services.RegisterApplicationLayerServices(builder.Configuration);

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .WithMethods(HttpMethods.Get)
        .AllowAnyHeader());

    app.UseAuthorization();

    app.MapControllers();

    await DatabaseMigration.Run(app);

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
