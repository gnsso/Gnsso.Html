using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gnsso.Html
{
    public static class ConverterCache
    {
        private static KeyedCache<Type, object[], HtmlConverter> cache;

        static ConverterCache()
        {
            cache = cache ?? new KeyedCache<Type, object[], HtmlConverter>(ConverterArgsEqualityComparer.Instance)
            {
                DefaultValueKey = Array.Empty<object>()
            };
        }

        public static HtmlConverter GetOrAdd(Type type, object[] parameters)
        {
            return cache.GetOrAdd(type, parameters, GetValue);
        }

        private static HtmlConverter GetValue(Type type, object[] parameters)
        {
            if (parameters.Length != 0)
                return (HtmlConverter)Activator.CreateInstance(type, parameters);
            else return (HtmlConverter)Activator.CreateInstance(type);
        }

        private class ConverterArgsEqualityComparer : IEqualityComparer<object[]>
        {
            public static readonly ConverterArgsEqualityComparer Instance = new ConverterArgsEqualityComparer();

            public bool Equals(object[] x, object[] y)
            {
                if (x == y)
                {
                    return true;
                }
                if (x == null || y == null)
                {
                    return false;
                }
                if (x.Length != y.Length)
                {
                    return false;
                }
                for (int i = 0; i < x.Length; i++)
                {
                    if (!Equals(x[i], y[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(object[] obj)
            {
                return 0;
            }
        }
    }
}
