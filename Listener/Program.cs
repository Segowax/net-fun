using Listener;
using Listener.Options;
using Microsoft.Extensions.Configuration;

Rs232? communicationListener = null;
SupabaseDataWriter? supabaseDataWriter = null;
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

    var supabaseOptions = configuration
        .GetSection(MySupabaseOptions.Supabase)
        .Get<MySupabaseOptions>()
            ?? throw new ArgumentNullException($"{nameof(MySupabaseOptions)} are not set.");

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