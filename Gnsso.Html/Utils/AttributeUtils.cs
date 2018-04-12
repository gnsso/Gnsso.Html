using System;
using System.Linq;

namespace Gnsso.Html
{
    public static class AttributeUtils
    {
        public static T GetTypeAttribute<T>(Type type) where T : Attribute
        {
            return GetTypeAttribute<T>(type, false);
        }

        public static T GetTypeAttribute<T>(Type type, bool inherit) where T : Attribute
        {
            ValidationUtils.NotNull(type, nameof(type));

            var attributes = type.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length == 1) return (T)attributes[0];
            return null;
        }

        public static bool TryGetTypeAttribute<T>(Type type, out T attribute) where T : Attribute
        {
            try
            {
                return (attribute = GetTypeAttribute<T>(type)) != null;
            }
            catch
            {
                attribute = null;
                return false;
            }
        }

        public static bool TryGetTypeAttribute<T>(Type type, bool inherit, out T attribute) where T : Attribute
        {
            try
            {
                return (attribute = GetTypeAttribute<T>(type, inherit)) != null;
            }
            catch
            {
                attribute = null;
                return false;
            }
        }

        public static T GetPropertyAttribute<T>(Type declaringType, string propertyName) where T : Attribute
        {
            return GetPropertyAttribute<T>(declaringType, propertyName, false);
        }

        public static T GetPropertyAttribute<T>(Type declaringType, string propertyName, bool inherit) where T : Attribute
        {
            ValidationUtils.NotNull(declaringType, nameof(declaringType));
            ValidationUtils.NotNull(propertyName, nameof(propertyName));

            if (TypeUtils.TryGetProperty(declaringType, propertyName, out var propertyInfo))
            {
                var propertyAttributes = propertyInfo.GetCustomAttributes(typeof(T), inherit);
                if (propertyAttributes.Cast<T>().TryGetSingle(out T attribute))
                    return attribute;
            }
            return null;
        }

        public static bool TryGetPropertyAttribute<T>(Type declaringType, string propertyName, out T attribute) where T : Attribute
        {
            try
            {
                return (attribute = GetPropertyAttribute<T>(declaringType, propertyName)) != null;
            }
            catch
            {
                attribute = null;
                return false;
            }
        }

        public static bool TryGetPropertyAttribute<T>(Type declaringType, string propertyName, bool inherit, out T attribute) where T : Attribute
        {
            try
            {
                return (attribute = GetPropertyAttribute<T>(declaringType, propertyName, inherit)) != null;
            }
            catch
            {
                attribute = null;
                return false;
            }
        }
    }
}
