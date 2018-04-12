using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsso.Html
{
    internal static class AttributeCache
    {
        private static KeyedCache<Type, HtmlObjectAttribute> objectAttributeCache;
        private static KeyedCache<Type, string, HtmlPropertyAttribute> propertyAttributeCache;
        private static KeyedCache<Type, string, HtmlConverterAttribute> converterAttributeCache;
        private static KeyedCache<Type, string, HtmlRegexAttribute> regexAttributeCache;

        static AttributeCache()
        {
            objectAttributeCache = objectAttributeCache ?? new KeyedCache<Type, HtmlObjectAttribute>();
            propertyAttributeCache = propertyAttributeCache ?? new KeyedCache<Type, string, HtmlPropertyAttribute>();
            converterAttributeCache = converterAttributeCache ?? new KeyedCache<Type, string, HtmlConverterAttribute>();
            regexAttributeCache = regexAttributeCache ?? new KeyedCache<Type, string, HtmlRegexAttribute>();
        }

        public static HtmlObjectAttribute GetOrAddObjectAttribute(Type objectType)
        {
            return objectAttributeCache.GetOrAdd(objectType, GetObjectAttributeValue);
        }

        public static HtmlPropertyAttribute GetOrAddPropertyAttribute(Type declaringType, string propertyName)
        {
            return propertyAttributeCache.GetOrAdd(declaringType, propertyName, GetPropertyAttributeValue);
        }

        public static HtmlConverterAttribute GetOrAddConverterAttribute(Type declaringType, string propertyName)
        {
            return converterAttributeCache.GetOrAdd(declaringType, propertyName, GetConverterAttributeValue);
        }

        public static HtmlRegexAttribute GetOrAddRegexAttribute(Type declaringType, string propertyName)
        {
            return regexAttributeCache.GetOrAdd(declaringType, propertyName, GetRegexAttributeValue);
        }

        public static bool TryGetOrAddObjectAttribute(Type objectType, out HtmlObjectAttribute attribute)
        {
            return (attribute = GetOrAddObjectAttribute(objectType)) != null;
        }

        public static bool TryGetOrAddPropertyAttribute(Type declaringType, string propertyName, out HtmlPropertyAttribute attribute)
        {
            return (attribute = GetOrAddPropertyAttribute(declaringType, propertyName)) != null;
        }

        public static bool TryGetOrAddConverterAttribute(Type declaringType, string propertyName, out HtmlConverterAttribute attribute)
        {
            return (attribute = GetOrAddConverterAttribute(declaringType, propertyName)) != null;
        }

        public static bool TryGetOrAddRegexAttribute(Type declaringType, string propertyName, out HtmlRegexAttribute attribute)
        {
            return (attribute = GetOrAddRegexAttribute(declaringType, propertyName)) != null;
        }

        private static HtmlObjectAttribute GetObjectAttributeValue(Type objectType)
        {
            return AttributeUtils.GetTypeAttribute<HtmlObjectAttribute>(objectType);
        }

        private static HtmlPropertyAttribute GetPropertyAttributeValue(Type declaringType, string propertyName)
        {
            return AttributeUtils.GetPropertyAttribute<HtmlPropertyAttribute>(declaringType, propertyName);
        }

        private static HtmlConverterAttribute GetConverterAttributeValue(Type declaringType, string propertyName)
        {
            return AttributeUtils.GetPropertyAttribute<HtmlConverterAttribute>(declaringType, propertyName);
        }

        private static HtmlRegexAttribute GetRegexAttributeValue(Type declaringType, string propertyName)
        {
            return AttributeUtils.GetPropertyAttribute<HtmlRegexAttribute>(declaringType, propertyName);
        }
    }
}
