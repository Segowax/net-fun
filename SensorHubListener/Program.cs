using Listener;
using Listener.Options;
using Microsoft.Extensions.Configuration;

Rs232? communicationListener = null;
AzureEventHub? azureEventHub = null;
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

try
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfiguration configuration = builder.Build();

    var rs232Options = configuration
        .GetSection(Rs232Options.Rs232)
        .Get<Rs232Options>()
        ?? throw new ArgumentNullException($"{nameof(Rs232Options)} are not set.");
    var eventHubOptions = configuration
        .GetSection(EventHubOptions.EventHub)
        .Get<EventHubOptions>()
        ?? throw new ArgumentNullException($"{nameof(EventHubOptions)} are not set.");

    communicationListener = new Rs232(rs232Options);

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

    azureEventHub = new AzureEventHub(eventHubOptions);
    BufforToSend.rs232Data.ItemAdded += async (sender, args) =>
    {
        try
        {
            await azureEventHub.SendEvent(args.Value);
            BufforToSend.rs232Data.Remove(args.Key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during ItemAdded - {ex.Message}");
        }
    };

    while (!cancellationTokenSource.Token.IsCancellationRequested)
    {
        Console.WriteLine("Press to l to look up error list, x to kill and exit.");
        var button = Console.ReadKey();
        Console.WriteLine();

        if (button.Key == ConsoleKey.L)
        {
            foreach (var data in BufforToSend.rs232Data.Error.Values)
            {
                Console.WriteLine(data);
            }
        }
        else if (button.Key == ConsoleKey.X)
        {
            communicationListener.Dispose();
            await azureEventHub.DisposeAsync();
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
    if (azureEventHub != null)
        await azureEventHub.DisposeAsync();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}