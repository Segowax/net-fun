using Listener;

var communicationListener = new Rs232();

#pragma warning disable CS4014 // To wywołanie nie jest oczekiwane, dlatego wykonywanie bieżącej metody będzie kontynuowane do czasu ukończenia wywołania
Task.Run(communicationListener.Listen);
#pragma warning restore CS4014 // To wywołanie nie jest oczekiwane, dlatego wykonywanie bieżącej metody będzie kontynuowane do czasu ukończenia wywołania


while (true)
{
    Console.WriteLine("Press to q to look up current list, press x to kill");
    var button = Console.ReadKey();

    if (button.Key == ConsoleKey.Q)
    {
        foreach (var data in communicationListener.rs232Data.Values)
        {
            Console.WriteLine(data);
        }
    }
    if (button.Key == ConsoleKey.X)
    {
        communicationListener.Dispose();
        break;
    }
}
