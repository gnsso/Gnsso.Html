using System;
using System.Reflection;

namespace Gnsso.Html
{
    public static class TypeUtils
    {
        public static bool TryGetProperty(Type type, string propertyName, out PropertyInfo propertyInfo)
        {
            return (propertyInfo = type.GetProperty(propertyName ?? string.Empty)) != null;
        }
    }
}
