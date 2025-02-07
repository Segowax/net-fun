using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
var conn = builder.Configuration.GetSection("AzureAppConfigurationConnectionString");
builder.Configuration.AddAzureAppConfiguration(
    cfg => cfg.Connect(conn.Value));
#else
builder.Configuration.AddAzureAppConfiguration(options =>
    options.Connect(
        new Uri(builder.Configuration["AppConfig:Endpoint"] ?? throw new ArgumentNullException("AppConfig:Endpoint")),
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
