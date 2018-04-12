using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Gnsso.Html
{
    public static class KeyedCache
    {
        public static readonly string DefaultStringKey = "<empty>";
        public static T[] DefaultArray<T>() => new T[0];
    }

    public class KeyedCache<TKey, TValue>
    {
        private IEqualityComparer<TKey> keyComparer;
        private ConcurrentDictionary<TKey, TValue> dictionary;

        public TKey DefaultKey { get; set; }

        public KeyedCache() : this(EqualityComparer<TKey>.Default)
        {
            
        }

        public KeyedCache(IEqualityComparer<TKey> keyComparer)
        {
            dictionary = new ConcurrentDictionary<TKey, TValue>(this.keyComparer = keyComparer);
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var useKey = keyComparer.Equals(key, default) ? DefaultKey : key;
            return dictionary.GetOrAdd(useKey, valueFactory != null ? valueFactory(useKey) : default);
        }
    }

    public class KeyedCache<TKey, TValueKey, TValue>
    {
        private IEqualityComparer<TKey> keyComparer;
        private IEqualityComparer<TValueKey> valueKeyComparer;
        private ConcurrentDictionary<TKey, ConcurrentDictionary<TValueKey, TValue>> dictionary;

        public TKey DefaultKey { get; set; }
        public TValueKey DefaultValueKey { get; set; }

        public KeyedCache() : this(EqualityComparer<TKey>.Default, EqualityComparer<TValueKey>.Default)
        {

        }

        public KeyedCache(IEqualityComparer<TKey> keyComparer) : this(keyComparer, EqualityComparer<TValueKey>.Default)
        {

        }

        public KeyedCache(IEqualityComparer<TValueKey> valueKeyComparer) : this(EqualityComparer<TKey>.Default, valueKeyComparer)
        {

        }

        public KeyedCache(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValueKey> valueKeyComparer)
        {
            dictionary = new ConcurrentDictionary<TKey, ConcurrentDictionary<TValueKey, TValue>>(this.keyComparer = keyComparer);
            this.valueKeyComparer = valueKeyComparer;
        }

        public TValue GetOrAdd(TKey key, TValueKey valueKey, Func<TKey, TValueKey, TValue> valueFactory)
        {
            var useKey = keyComparer.Equals(key, default) ? DefaultKey : key;
            var useValueKey = valueKeyComparer.Equals(valueKey, default) ? DefaultValueKey : valueKey;
            var valueDictionary = dictionary.GetOrAdd(useKey, k => new ConcurrentDictionary<TValueKey, TValue>(valueKeyComparer));
            return valueDictionary.GetOrAdd(useValueKey, valueFactory != null ? valueFactory(useKey, useValueKey) : default);
        }
    }
}
