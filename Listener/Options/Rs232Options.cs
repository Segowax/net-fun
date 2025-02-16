namespace Listener.Options
{
    internal class Rs232Options
    {
        public const string Rs232 = "Rs232";

        public required string Port { get; set; }
        public required int BaudRate { get; set; }
    }
}
