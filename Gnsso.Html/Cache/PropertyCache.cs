using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gnsso.Html
{
    public static class PropertyCache
    {
        private static KeyedCache<Type, string, PropertyInfo> cache;

        static PropertyCache()
        {
            cache = cache ?? new KeyedCache<Type, string, PropertyInfo>();
        }

        public static PropertyInfo GetOrAdd(Type declaringType, string propertyName)
        {
            return cache.GetOrAdd(declaringType, propertyName, (o, p) => o.GetProperty(p));
        }
    }
}
