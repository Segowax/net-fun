using System.Collections.Concurrent;

namespace Listener
{
    internal static class BufforToSend
    {
        internal static ObservableConcurrentDictionary<string, string> rs232Data = new();
    }

    internal class ObservableConcurrentDictionary<TKey, TValue> where TKey : notnull
    {
        internal readonly ConcurrentDictionary<TKey, TValue> Data = [];
        internal event EventHandler<KeyValuePair<TKey, TValue>>? ItemAdded;

        internal bool TryAdd(TKey key, TValue value)
        {
            if (Data.TryAdd(key, value))
            {
                OnItemAdded(new KeyValuePair<TKey, TValue>(key, value));

                return true;
            }

            return false;
        }

        internal void Remove(TKey key)
        {
            Data.Remove(key, out _);
        }

        protected virtual void OnItemAdded(KeyValuePair<TKey, TValue> e)
        {
            ItemAdded?.Invoke(this, e);
        }
    }
}