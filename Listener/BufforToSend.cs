using System.Collections.Concurrent;

namespace Listener
{
    internal static class BufforToSend
    {
        internal static ConcurrentDictionary<string, string> rs232Data = [];
    }
}
