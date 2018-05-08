using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gnsso.Html
{
    public static class ParserCache
    {
        private static KeyedCache<Type, HtmlParser> cache;

        static ParserCache()
        {
            cache = cache ?? new KeyedCache<Type, HtmlParser>();
        }

        public static HtmlParser GetOrAdd(Type type)
        {
            return cache.GetOrAdd(type, GetValue);
        }

        private static HtmlParser GetValue(Type objType)
        {
            if (!AttributeCache.TryGetOrAddObjectAttribute(objType, out var objAttr))
                return null;

            var parser = new HtmlParser(objType, objAttr.Selector);

            foreach (var propInfo in objType.GetProperties())
            {
                var propName = propInfo.Name;
                if (!AttributeCache.TryGetOrAddPropertyAttribute(objType, propName, out var propAttr))
                    continue;

                var propSelector = propAttr.Selector;
                var propConverter = HtmlConverter.Default;

                if (AttributeCache.TryGetOrAddConverterAttribute(objType, propName, out var convAttr))
                {
                    propConverter = ConverterCache.GetOrAdd(convAttr.Type, convAttr.Args);
                }

                var hasRegex = AttributeCache.TryGetOrAddRegexAttribute(objType, propName, out var regexAttr);

                parser.Set(propName, propSelector, value =>
                {
                    if (hasRegex && Regex.IsMatch(value, regexAttr.Pattern))
                    {
                        if (regexAttr.Replacement != null)
                            value = Regex.Replace(value, regexAttr.Pattern, regexAttr.Replacement);
                        else
                            value = Regex.Match(value, regexAttr.Pattern).Value;
                    }
                    
                    return propConverter.Convert(value, propInfo.PropertyType);
                });
            }
            
            return parser;
        }
    }
}
