using Listener;
using Listener.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

Rs232? communicationListener = null;
SupabaseDataWriter? supabaseDataWriter = null;
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

try
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<Program>(optional: true)
        .AddEnvironmentVariables();
    IConfiguration configuration = builder.Build();

    var services = new ServiceCollection();
    services.AddOptions<Rs232Options>()
        .Bind(configuration.GetSection(Rs232Options.Rs232))
        .Validate(options => !string.IsNullOrWhiteSpace(options.Port), "Rs232:Port is required.")
        .Validate(options => options.BaudRate > 0, "Rs232:BaudRate must be greater than zero.");
    services.AddOptions<MySupabaseOptions>()
        .Bind(configuration.GetSection(MySupabaseOptions.Supabase))
        .Validate(options => Uri.TryCreate(options.ProjectURL, UriKind.Absolute, out _), "Supabase:ProjectURL must be a valid absolute URL.")
        .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey), "Supabase:ApiKey is required.");

    using var serviceProvider = services.BuildServiceProvider();
    var rs232Options = serviceProvider.GetRequiredService<IOptions<Rs232Options>>().Value;
    var supabaseOptions = serviceProvider.GetRequiredService<IOptions<MySupabaseOptions>>().Value;

    communicationListener = new Rs232(rs232Options);
    supabaseDataWriter = new SupabaseDataWriter(supabaseOptions);

#pragma warning disable CS4014
    Task.Run(async () =>
    {
        try
        {
            await communicationListener.Listen();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            cancellationTokenSource.Cancel();
        }
    });
#pragma warning restore CS4014

    await supabaseDataWriter.InitializeClient();
    BufforToSend.rs232Data.ItemAdded += supabaseDataWriter.SensorDataWrite;

    while (!cancellationTokenSource.Token.IsCancellationRequested)
    {
        Console.WriteLine("Press to l to look up rs232 error list, press s to look up supabase error list, x to kill and exit.");
        var button = Console.ReadKey();
        Console.WriteLine();

        if (button.Key == ConsoleKey.L)
        {
            foreach (var data in BufforToSend.rs232Data.Error.Values)
            {
                Console.WriteLine(data);
            }
        }
        else if (button.Key == ConsoleKey.S)
        {
            foreach (var data in SupabaseDataWriter.Error.Values)
            {
                Console.WriteLine(data);
            }
        }
        else if (button.Key == ConsoleKey.X)
        {
            communicationListener.Dispose();
            supabaseDataWriter.DisposeClient();
            break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    communicationListener?.Dispose();
    supabaseDataWriter?.DisposeClient();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}