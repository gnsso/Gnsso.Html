using System;
using System.Collections.Generic;
using System.Linq;

namespace Gnsso.Html
{
    internal static class EnumerableExtensions
    {
        public static bool TryGetSingle<T>(this IEnumerable<T> source, out T value)
        {
            return source.TryGet(Enumerable.Single, out value);
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> source, out T value)
        {
            return source.TryGet(Enumerable.First, out value);
        }

        private static bool TryGet<T>(this IEnumerable<T> source, Func<IEnumerable<T>, T> func, out T value)
        {
            value = default;
            if (source == null) return false;
            try
            {
                value = func(source);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
