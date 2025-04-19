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

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.UseCors(builder => builder
        .AllowAnyOrigin()
        .WithMethods(HttpMethods.Get));

    app.UseDefaultFiles(new DefaultFilesOptions
    {
        DefaultFileNames = new List<string> { "index.html" }
    });
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapControllers();

    await DatabaseMigration.Run(app);

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
