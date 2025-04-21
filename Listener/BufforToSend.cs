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
        internal readonly ConcurrentDictionary<TKey, TValue> Error = [];
        internal event EventHandler<KeyValuePair<TKey, TValue>>? ItemAdded;

        internal bool TryAdd(TKey key, TValue value)
        {
            if (value is not null && value is string valueString)
            {
                if (valueString.StartsWith('{') && valueString.EndsWith('}'))
                {
                    if (Data.TryAdd(key, (TValue)(object)valueString))
                    {
                        OnItemAdded(new KeyValuePair<TKey, TValue>(key, (TValue)(object)valueString));

                        return true;
                    }
                }
            }

            Error.TryAdd(key, value);

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