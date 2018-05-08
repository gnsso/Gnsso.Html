using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsso.Html
{
    public static class SelectorCache
    {
        private static KeyedCache<string, HtmlObjectSelector> objectCache;
        private static KeyedCache<string, HtmlPropertySelector> propertyCache;

        static SelectorCache()
        {
            objectCache = objectCache ?? new KeyedCache<string, HtmlObjectSelector> { DefaultKey = KeyedCache.DefaultStringKey };
            propertyCache = propertyCache ?? new KeyedCache<string, HtmlPropertySelector> { DefaultKey = KeyedCache.DefaultStringKey };
        }

        public static HtmlObjectSelector GetOrAddObject(string objSelector)
        {
            return objectCache.GetOrAdd(objSelector, selector => new HtmlObjectSelector(selector));
        }

        public static HtmlPropertySelector GetOrAddProperty(string propSelector)
        {
            return propertyCache.GetOrAdd(propSelector, selector => new HtmlPropertySelector(selector));
        }
    }
}
