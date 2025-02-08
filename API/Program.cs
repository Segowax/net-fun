using AzureConfigurations;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddMyAzureAppConfigurations(builder.Configuration);

    builder.Services.RegisterMyAzureAppConfigurations();
    builder.Services.RegisterMyAppInsightsConfigurations();

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
