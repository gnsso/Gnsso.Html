using System;

namespace Gnsso.Html
{
    public static class ConverterUtils
    {
        public static object Convert(string sourceValue, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return sourceValue;
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, sourceValue, true);
            }

            if (TryGetUnderlyingType(targetType, out var underlyingType))
            {
                return Convert(sourceValue, underlyingType);
            }

            if (IsPrimitive(targetType))
            {
                return System.Convert.ChangeType(sourceValue, targetType);
            }

            return null;
        }

        public static bool IsPrimitive(Type targetType)
        {
            return targetType == typeof(string) ||
                targetType.IsEnum ||
                targetType == typeof(bool) ||
                targetType == typeof(byte) ||
                targetType == typeof(char) ||
                targetType == typeof(decimal) ||
                targetType == typeof(double) ||
                targetType == typeof(float) ||
                targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(sbyte) ||
                targetType == typeof(short) ||
                targetType == typeof(uint) ||
                targetType == typeof(ulong) ||
                targetType == typeof(ushort) ||
                targetType == typeof(DateTime);
        }

        public static bool TryGetUnderlyingType(Type targetType, out Type underlyingType)
        {
            return (underlyingType = Nullable.GetUnderlyingType(targetType)) != null;
        }
    }
}
