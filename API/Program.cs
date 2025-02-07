using Azure.Identity;

try
{
    var builder = WebApplication.CreateBuilder(args);

#if DEBUG
    var conn = builder.Configuration["AzureAppConfigurationConnectionString"] ?? throw new ArgumentNullException("AzureAppConfigurationConnectionString");
    builder.Configuration.AddAzureAppConfiguration(
        options => options.Connect(conn));
#else
builder.Configuration.AddAzureAppConfiguration(options =>
    options.Connect(
        new Uri(builder.Configuration["AppConfig"] ?? throw new ArgumentNullException("AppConfig")),
        new ManagedIdentityCredential()));
#endif
    builder.Services.AddAzureAppConfiguration();

    // Add Application Insights telemetry collection.
    builder.Services.AddApplicationInsightsTelemetry();

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

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
