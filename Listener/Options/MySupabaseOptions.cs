namespace Listener.Options
{
    internal class MySupabaseOptions
    {
        public const string Supabase = nameof(Supabase);
        public required string ProjectURL { get; set; }
        public required string ApiKey { get; set; }
    }
}
