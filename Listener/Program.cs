using Listener;
using Listener.Options;
using Microsoft.Extensions.Configuration;

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

var communicationListener = new Rs232(rs232Options);

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
    };
});
#pragma warning restore CS4014

var azureEventHub = new AzureEventHub(eventHubOptions);
BufforToSend.rs232Data.ItemAdded += async (sender, args) =>
{
    await azureEventHub.SendEvent(args.Value);
    BufforToSend.rs232Data.Remove(args.Key);
};

while (true)
{
    Console.WriteLine("Press to q to look up current list, press x to kill.");
    var button = Console.ReadKey();
    Console.WriteLine();

    if (button.Key == ConsoleKey.Q)
    {
        foreach (var data in BufforToSend.rs232Data.Data.Values)
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